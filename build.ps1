Param(
	$target
)

# VSSetup module is needed to resolve the build environment
Install-Module VSSetup -Scope CurrentUser

&.\packages\psake.4.9.0\tools\psake\psake.ps1 build-psake.ps1 $target
