SET buildType= "build_%1"
IF %buildType% == "build_" SET buildType="build_local"
rake -f build_tools\Rakefile.rb %buildType%
