$ErrorActionPreference = "Stop";

$nuget_path = [System.IO.Path]::GetFullPath("$PSScriptRoot\..\NugetFeed");

if (!(Test-Path ".git")) {
    throw "Must be under source control (to determine version)";
}

$tag = (git describe | Out-String).Split("-")[0].TrimEnd();

if($tag[0] -ne "v"){
    throw "Must be tagged with a version (like v1.0)";
}

$tag = $tag.Substring(1);

if(!($tag -match "^\d+\.\d+(\.\d+)?$")){
    throw "Version tag must contain two (like v1.0) or three numerical segments (like v1.0.0), was `"v" + $tag + "`"";
}

if($tag -match "^\d+\.\d+$"){
    $tag = "$tag.0";
}

$build_version = 1;

foreach($project_file in Get-ChildItem -File -Filter *.csproj -Recurse -Depth 2 -Exclude Website.*,Tests.*,Cloudy.CMS.MongoDB.Integrations.JsonDotNet) {
    $package_id = [System.IO.Path]::GetFileNameWithoutExtension($project_file.Name);

    $latest_package_name = Get-ChildItem -Path $nuget_path -File | Where-Object { $_.Name -match "^$package_id\.$tag\.\d+\.nupkg$" } | Sort-Object -Property @{Expression={$_.Name.Substring($package_id.Length).Split(".")[4] -as [int]}} -Descending;

    if($latest_package_name.Count -gt 0) {
        $this_build_version = $latest_package_name[0].Name.Substring($package_id.Length).Split(".")[4] -as [int];

		$this_build_version++;

		if($this_build_version -gt $build_version){
			$build_version = $this_build_version;
		}
    }
}

$version = "$tag.$build_version";

echo $version;

$command = "dotnet build --verbosity quiet /p:Version=$version"
Write-Host $command -ForegroundColor Yellow
Invoke-Expression $command

foreach($project_file in Get-ChildItem -File -Filter *.csproj -Recurse -Depth 2 -Exclude Website.*,Tests.*) {
	$command = "dotnet pack --no-build ""$($project_file.FullName)"" --output ""$nuget_path"" /p:PackageVersion=$version"
	Write-Host $command -ForegroundColor Yellow
	Invoke-Expression $command
}

[console]::beep(1000,500)