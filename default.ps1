properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\build"
    $source_dir = "$base_dir\Regard.Endpoint"
    $package_dir = "$base_dir\packages"
    $framework_dir =  (Get-ProgramFiles) + "\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
    $config = "release"
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

task compile -depends clean {
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

function Get-ProgramFiles
{
    #TODO: Someone please come up with a better way of detecting this - Tried http://msmvps.com/blogs/richardsiddaway/archive/2010/02/26/powershell-pack-mount-specialfolder.aspx and some enums missing
    #      This is needed because of this http://www.mattwrock.com/post/2012/02/29/What-you-should-know-about-running-ILMerge-on-Net-45-Beta-assemblies-targeting-Net-40.aspx (for machines that dont have .net 4.5 and only have 4.0)
    if (Test-Path "C:\Program Files (x86)") {
        return "C:\Program Files (x86)"
    }
    return "C:\Program Files"
}