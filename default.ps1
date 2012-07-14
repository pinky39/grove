# acknowledgement: 
# This psake build script is based upon rhino-etl build script https://github.com/ayende/rhino-etl

Framework "4.0"

properties { 
  $base_dir  = resolve-path .  
  $source_dir = "$base_dir\source"
  $lib_dir = "$base_dir\lib"
  $tests_dir = "$base_dir\tests"
  $build_dir = "$base_dir\build" 
  $buildartifacts_dir = "$build_dir\" 
  $sln_file = "$base_dir\Grove.sln"   
  $product = "Magicgrove"
  $humanReadableversion = "1.4"
  $tools_dir = "$base_dir\lib"	
  $release_dir = "$base_dir\release"   
  $media_dir = "$base_dir\media"
} 

task default -depends DoReleaseTest
task defaultNoTest -depends DoRelease

task Clean { 
  remove-item -force -recurse $buildartifacts_dir -ErrorAction SilentlyContinue 
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue   
} 

task Init -depends Clean { 		
	new-item $release_dir -itemType directory 
	new-item $buildartifacts_dir -itemType directory 
	
	cp $tools_dir\xunit\*.* $build_dir	
} 

task Compile -depends Init { 
  & msbuild "$sln_file" "/p:OutDir=$build_dir\\" /p:Configuration=Release
    
  if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute msbuild"
  }
} 

task Test -depends DoRelease {
  $old = pwd
  cd $build_dir
  &.\xunit.console.clr4.x86.exe "$build_dir\Grove.Tests.dll"
  if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute tests"
    }  
  cd $old
}

task DoReleaseTest -depends Test {			
}

task DoRelease -depends Compile {			
	
	cp "$build_dir\grove.exe" $release_dir	
	cp "$base_dir\readme.txt" $release_dir
	cp "$base_dir\cards.txt" $release_dir
	cp -recurse "$media_dir" $release_dir
	
	$old = pwd
	cd $release_dir
	
	& $tools_dir\zip.exe -9 -A -R $release_dir\magicgrove-$humanReadableversion.zip *
			
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute ZIP command"
    }			
	cd $old
}
