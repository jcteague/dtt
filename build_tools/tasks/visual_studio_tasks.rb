require File.join('./','build_tools','config.rb')
require 'albacore'
begin
    require 'FileUtils'
    def copy(source, dest)
    	FileUtils.copy source, dest
    end
rescue LoadError
    def copy(source, dest)
    	sh "cp #{source}, #{dest}"
    end
end

namespace :visual_studio do

  task :build => [
    :msbuild, 
    :copy_in_destination, 
    :create_extension_manifest
  ]

  msbuild :msbuild do |msb|
    msb.properties = { :configuration => :Release }
    msb.targets = [ :Clean, :Build ]
    msb.solution = VisualStudioSolution
  end

  task :copy_in_destination do
    copy File.join(VisualStudioReleaseFolder, 'TeamNotification_Package.vsix'), '.'
  end

  task :create_extension_manifest do
    file_hash = convert_xml_file_to_hash(VisualStudioManifest)
    vs_extension_file = {
			  "name" => file_hash["Vsix"]["Identifier"]["Name"],
	    		  "version" => file_hash["Vsix"]["Identifier"]["Version"],
			  "file_name" => "TeamNotification_Package.vsix"
    			}
    write_hash_to_json_file(File.join(RestServiceData, 'vs.extension.json'), vs_extension_file)
  end

end
