Framework "4.7.2"

properties {
  $base_dir  = resolve-path .
  $source_dir = "$base_dir\source"  
  $tests_dir = "$base_dir\tests"
  $build_dir = "$base_dir\build"
  $buildartifacts_dir = "$build_dir\"
  $sln_file = "$base_dir\Grove.sln"
  $product = "Magicgrove"
  $humanReadableversion = "4.0"
  $tools_dir = "$base_dir\tools"
  $packages_dir = "$base_dir\packages"
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

	cp $packages_dir\xunit.runner.console.2.4.1\tools\net472\*.* $build_dir	
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
  ./xunit.console.exe Grove.Tests.dll -verbose
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
  cp "$build_dir\ugrove.exe" $release_dir
  cp "$build_dir\settings.json" $release_dir
  cp "$build_dir\Lucene.Net.dll" $release_dir
  cp "$build_dir\log4net.dll" $release_dir
  cp "$build_dir\Castle.Core.dll" $release_dir
  cp "$build_dir\Castle.Windsor.dll" $release_dir
  cp "$build_dir\System.Windows.Interactivity.dll" $release_dir
  cp "$build_dir\Caliburn.Micro.dll" $release_dir
  cp "$build_dir\Ionic.Zip.dll" $release_dir
  cp "$build_dir\Newtonsoft.Json.dll" $release_dir
  
  cp "$base_dir\README.md" $release_dir
  cp "$base_dir\LICENCE" $release_dir
  cp "$base_dir\HISTORY.md" $release_dir
  cp "$base_dir\CARDS.md" $release_dir

  cp -recurse "$media_dir\decks" "$release_dir\media\decks"
  new-item "$release_dir\media\logs" -itemType directory
  new-item "$release_dir\media\saved" -itemType directory
  cp -recurse "$media_dir\sets" "$release_dir\media\sets"
  cp "$media_dir\player-names.txt" "$release_dir\media"

  & $zip a "$release_dir\media\avatars.zip" "$media_dir\avatars\*"
  & $zip a "$release_dir\media\tournament.zip" "$media_dir\tournament\*"
  & $zip a "$release_dir\media\images.zip" "$media_dir\images\*"
  & $zip a "$release_dir\magicgrove-$humanReadableversion.zip" "$release_dir\*"
}
