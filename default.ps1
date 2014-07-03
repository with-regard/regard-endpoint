properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\build"
    $source_dir = "$base_dir\Regard.Endpoint"
    $package_dir = "$base_dir\packages"
    $framework_dir =  (Get-ProgramFiles) + "\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
    $config = "release"
    $build_number = if ($env:build_number -eq $null) { "0.0.0.1" } else { $env:build_number }
}

task default -depends package

task clean {
	Write-Host "Creating artifacts directory" -ForegroundColor Green

	rd $build_dir\* -rec -force -ErrorAction SilentlyContinue | out-null
	rd $base_dir\Regard.Endpoint\bin\* -rec -force -ErrorAction SilentlyContinue | out-null
	rd $base_dir\Regard.Endpoint\obj\* -rec -force -ErrorAction SilentlyContinue | out-null
	rd $base_dir\Regard.Endpoint.Tests\bin\* -rec -force -ErrorAction SilentlyContinue | out-null
	rd $base_dir\Regard.Endpoint.Tests\obj\* -rec -force -ErrorAction SilentlyContinue | out-null
	
	Write-Host "Cleaning SLN" -ForegroundColor Green
	Exec { msbuild $base_dir\Regard.Endpoint.sln /t:Clean /p:Configuration=$config } 
}

task version {
	Update-AssemblyInfofiles $build_number
}

task compile -depends clean, version {
    "Compiling"
    "   Regard.Endpoint.sln"
    
    exec { msbuild $base_dir\Regard.Endpoint.sln /p:Configuration=$config /tv:4.0 }
}

task test -depends compile {
    "Testing"
    
    exec { & $base_dir\packages\NUnit.Runners.2.6.3\tools\nunit-console.exe $base_dir\Regard.Endpoint.Tests\bin\$config\Regard.Endpoint.Tests.dll }
}

task package -depends test {
    "Packaging"
    "   Regard.Endpoint.sln"

    exec { msbuild $base_dir\Regard.Endpoint\Regard.Endpoint.csproj /t:Package /p:Configuration=$config /p:PackageLocation="$build_dir\package.zip" /tv:4.0  }
}

#-------------------------------------------------------------------------------
# Update version numbers of AssemblyInfo.cs
#-------------------------------------------------------------------------------
function Update-AssemblyInfoFiles ([string] $version) {
    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $assemblyVersion = 'AssemblyVersion("' + $version + '")';
    $fileVersion = 'AssemblyFileVersion("' + $version + '")';
    
    Get-ChildItem -r -filter AssemblyInfo.cs | ForEach-Object {
        $filename = $_.Directory.ToString() + '\' + $_.Name
        $filename + ' -> ' + $version
        
        # If you are using a source control that requires to check-out files before 
        # modifying them, make sure to check-out the file here.
        # For example, TFS will require the following command:
        # tf checkout $filename
    
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $filename
    }
}

function Get-ProgramFiles
{
    #TODO: Someone please come up with a better way of detecting this - Tried http://msmvps.com/blogs/richardsiddaway/archive/2010/02/26/powershell-pack-mount-specialfolder.aspx and some enums missing
    #      This is needed because of this http://www.mattwrock.com/post/2012/02/29/What-you-should-know-about-running-ILMerge-on-Net-45-Beta-assemblies-targeting-Net-40.aspx (for machines that dont have .net 4.5 and only have 4.0)
    if (Test-Path "C:\Program Files (x86)") {
        return "C:\Program Files (x86)"
    }
    return "C:\Program Files"
}

