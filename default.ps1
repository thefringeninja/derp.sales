properties { 
  $base_dir = resolve-path .
  $build_dir = "$base_dir\build"
  $package_dir = "$base_dir\packages"
  $release_dir = "$base_dir\Release"
  $project_name = "Derp.Sales"
  $sln_file = "$base_dir\$project_name.sln"
  $nuget_dir = "$base_dir\.nuget"
  $config = "Release"
  $run_tests = $true
}

include .\psake_ext.ps1

$framework = '4.0'
$version = "1"

if($env:BUILD_NUMBER -ne $null) {
	$build_number  = $env:BUILD_NUMBER
}
if($build_number -eq $null) {
	$build_number = "0"
}

$octo_api_key = $env:OCTO_API_KEY

$branch = git rev-parse --abbrev-ref HEAD

$semver = "$version.0.$build_number"

$should_label = $true

if ($branch -ne "master") {
	$semver = "$semver-develop"
}

task default -depends Package

task Clean {
  remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue
}

task Init -depends Clean {
	Generate-Assembly-Info `
		-file $base_dir\SharedAssemblyInfo.cs `
		-title "Derp Sales" `
		-description "Derp Sales" `
		-company "dERP" `
		-product "Derp Sales" `
		-version $version `
		-semver $semver `
		-copyright "Derp 2013" `

	new-item $release_dir -itemType directory 
	new-item $build_dir -itemType directory 
}

task PackageRestore -depends Clean {
	$package_dir = "$base_dir\packages"
    Get-SolutionPackages -package_dir $package_dir |% { &$nuget_dir\nuget.exe install $_ -o $package_dir }
}

task Compile -depends Init, PackageRestore {
  exec { msbuild $sln_file /target:Rebuild /p:"OutDir=$build_dir\;Configuration=$config" }
}

task Test -depends Compile -precondition { return $run_tests }{
	exec { &$build_dir\$project_name.Tests.exe $release_dir\Specifications.html }
}

task Package -depends Test {
	cp $build_dir\*.nupkg $release_dir

	gci -r -i *.nuspec "$nuget_dir" | Foreach-Object {
		$nuspec = copy-item -destination $build_dir -path $_ -passthru
		Update-NuspecDependencies -base_dir $base_dir -nuspec_path $nuspec
		
		%{ .$nuget_dir\nuget.exe pack $nuspec -basepath $base_dir -o $release_dir -version $semver -verbose }
	}
}