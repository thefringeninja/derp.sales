function Get-Git-Commit
{
	$gitLog = git log --oneline -1
	return $gitLog.Split(' ')[0]
}

function Get-Version-From-Git-Tag
{
  $gitTag = git describe --tags --abbrev=0
  if ($gitTag -eq $null) {
	$gitTag = "v0";
  }
  return $gitTag.Replace("v", "") + ".0"
}

function Generate-Assembly-Info
{
param(
	[string]$clsCompliant = "true",
	[string]$title, 
	[string]$description, 
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$version,
	[string]$semver,
	[string]$file = $(throw "file is a required parameter.")
)
  $commit = Get-Git-Commit
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CLSCompliantAttribute($clsCompliant )]
[assembly: AssemblyTitleAttribute(""$title"")]
[assembly: AssemblyDescriptionAttribute(""$description"")]
[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version.0.0"")]
[assembly: AssemblyInformationalVersionAttribute(""$semver / $commit"")]
[assembly: AssemblyFileVersionAttribute(""$version.0.0"")]
[assembly: AssemblyDelaySignAttribute(false)]
"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
	Write-Output $asmInfo > $file
}

function Update-NuspecDependencies
{
	param (
		[string] $base_dir = $(throw "base_dir is required"),
		[string] $nuspec_path = $(throw "nuspec is required")
	)
	if ([System.IO.File]::Exists($nuspec_path) -eq $false) {
		throw "$nuspec_path doesn't exist"
	}

	$packages = Get-SolutionPackageVersions -base_dir $base_dir -package_dir "$base_dir\packages"
	
	$nuspec = New-Object Xml
	$nuspec.Load($nuspec_path)
	if ($nuspec.package.metadata -eq $null -or $nuspec.package.metadata.dependencies -eq $null) {
		return
	}

	$nuspec.package.metadata.dependencies.dependency | foreach-object {
		if ($_.version -eq "`$version`$") {
			return
		}
		$_.SetAttribute("version", $packages[$_.id])
	}	
	
	$nuspec.Save($nuspec_path)

	return $nuspec_path
}

function Get-SolutionPackages {
	param(
		[string] $package_dir
	)

	$repositories_path = "$package_dir\repositories.config"
	$repositories = New-Object Xml
	$repositories.Load($repositories_path)

	return $repositories.repositories.repository | Select path | foreach-object {
		$path = $_.path
		resolve-path $package_dir\$path
	}

}

function Get-SolutionPackageVersions {
	param(
		[string] $package_dir,
		[string] $base_dir
	)
	
	$ret = @{}
	
	Get-SolutionPackages -package_dir $package_dir | ForEach-Object {
		$package_path = $_
		
		$packages = New-Object xml
		$packages.load($package_path)
		$packages.packages.package | ForEach-Object {
			$ret[$_.Id] = $_.Version
		}
	}

	return $ret
}