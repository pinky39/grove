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
  $humanReadableversion = "3.0"
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
  cp -recurse $media_dir $build_dir
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

  $zip = "$tools_dir\\7za.exe"

  cp "$build_dir\grove.exe" $release_dir
  cp "$base_dir\readme.txt" $release_dir
  cp "$base_dir\license.txt" $release_dir
	cp "$base_dir\release notes.txt" $release_dir
	cp "$base_dir\cards.txt" $release_dir

  cp -recurse "$media_dir\decks" "$release_dir\media\decks"
  new-item "$release_dir\media\logs" -itemType directory
  new-item "$release_dir\media\saved" -itemType directory
  cp -recurse "$media_dir\sets" "$release_dir\media\sets"
  cp "$media_dir\*.*" "$release_dir\media"

  & $zip a "$release_dir\media\avatars.zip" "$media_dir\avatars\*"
  & $zip a "$release_dir\media\tournament.zip" "$media_dir\tournament\*"
  & $zip a "$release_dir\media\images.zip" "$media_dir\images\*"

	& $zip a "$release_dir\magicgrove-$humanReadableversion.zip" "$release_dir\*"
}
