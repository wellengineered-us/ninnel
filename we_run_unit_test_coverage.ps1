#
#	Copyright ©2020-2022 WellEngineered.us, all rights reserved.
#	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
#

cls

$this_dir_path = [System.Environment]::CurrentDirectory
$src_dir_path = "$this_dir_path\src"
$output_dir_path = "$this_dir_path\output"

$build_flavor = "debug"
$build_tfm = "netcoreapp1.1"

$dotnet_dir_path = "C:\Program Files\dotnet"
$dotnet_file_name = "dotnet.exe"
$dotnet_exe = "$dotnet_dir_path\$dotnet_file_name"

$dotcover_dir_path = "C:\Program Files (x86)\JetBrains\Installations\dotCover07"
$dotcover_file_name = "dotCover.exe"
$dotcover_exe = "$dotcover_dir_path\$dotcover_file_name"
$DOT_COVER_EXIT_CODE_FOR_NEQ_ZERO = -3

echo "The operation is starting..."

### -- for assembly under test (testsuite_for_assembly_name),
### -- run the entry point in the test suite assembly (testsuite_assembly_name),
### -- filter and execute tests cases in sub namespace (testsuite_filter_sub_ns_frag)

$cover_configs = @(
	@("Ninnel.Middleware.Solder._netcoreapp_", "Ninnel.Middleware.UnitTests", "Solder._netcoreapp_._"),
	@("Ninnel.Middleware.Solder.Abstractions", "Ninnel.Middleware.UnitTests", "Solder.Abstractions._"),
	@("Ninnel.Middleware.Solder.Primitives", "Ninnel.Middleware.UnitTests", "Solder.Primitives._"),
	@("Ninnel.Middleware.Solder.Context", "Ninnel.Middleware.UnitTests", "Solder.Context._"),
	@("Ninnel.Middleware.Solder.Utilities", "Ninnel.Middleware.UnitTests", "Solder.Utilities._"),
	@("Ninnel.Middleware.Solder.Injection", "Ninnel.Middleware.UnitTests", "Solder.Injection._"),
	@("Ninnel.Middleware.Solder.Interception", "Ninnel.Middleware.UnitTests", "Solder.Interception._"),
	@("Ninnel.Middleware.Solder.Serialization", "Ninnel.Middleware.UnitTests", "Solder.Serialization._"),
	@("Ninnel.Middleware.Solder.Extensions", "Ninnel.Middleware.UnitTests", "Solder.Extensions._"),
	@("Ninnel.Middleware.Solder.Executive", "Ninnel.Middleware.UnitTests", "Solder.Executive._"))

foreach ($cover_config in $cover_configs)
{
	$testsuite_for_assembly_name = $cover_config[0]
	$testsuite_assembly_name = $cover_config[1]
	$testsuite_filter_sub_ns_frag = $cover_config[2]

	echo "Executing coverage configuration for: $testsuite_for_assembly_name"

	# -----------------------------------------------------------------------

	$testsuite_filter_namespace = "$testsuite_assembly_name.$testsuite_filter_sub_ns_frag"
	$testsuite_assembly_dir = "$src_dir_path\$testsuite_assembly_name\bin\$build_flavor\$build_tfm"
	$testsuite_assembly_path = "$testsuite_assembly_dir\$testsuite_assembly_name.dll"

	$testsuite_filter = "--where class=~$testsuite_filter_namespace.*" # requires the wildcard suffix for class name (use of underscore in unit tests keeps out sub namespaces)
	$coverage_filter = "+:$testsuite_for_assembly_name" # removed the wildcard suffix

	$coverage_output_dir_path = "$output_dir_path\$testsuite_assembly_name\$testsuite_filter_sub_ns_frag"
	$coverage_output_file_path_woext ="$coverage_output_dir_path\unit-test-coverage-report"

	$target_exe = "$dotnet_exe"
	$target_args = @("$testsuite_assembly_path", "$testsuite_filter")
	$target_wdir = "."

	if ((Test-Path -Path $coverage_output_dir_path))
	{
		Remove-Item $coverage_output_dir_path -recurse -force
	}

	New-Item -ItemType directory -Path $coverage_output_dir_path

	&$dotcover_exe analyse /Filters="$coverage_filter" `
		/TargetExecutable="$target_exe" `
		/TargetArguments="$target_args" `
		/TargetWorkingDir="$target_wdir" `
		/ReportType=HTML `
		/Output="$coverage_output_file_path_woext.html" > "$coverage_output_file_path_woext.log"

	if (!($LastExitCode -eq $null -or $LastExitCode -eq 0 -or $LastExitCode -eq $DOT_COVER_EXIT_CODE_FOR_NEQ_ZERO))
	{ echo "An error occurred during the operation."; return; }

	(New-Object -Com Shell.Application).Open("$coverage_output_file_path_woext.html")
}

echo "The operation completed successfully."