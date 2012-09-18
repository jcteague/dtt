require File.join('./','build_tools','config.rb')
require 'albacore'

namespace :visual_studio do

  task :build => [
    :msbuild, :copy_in_destination
  ]

  msbuild :msbuild do |msb|
    msb.properties = { :configuration => :Release }
    msb.targets = [ :Clean, :Build ]
    msb.solution = VisualStudioSolution
  end

  task :copy_in_destination do
    sh "cp #{File.join(VisualStudioReleaseFolder, 'TeamNotification_Package.vsix')} ."
  end

end
