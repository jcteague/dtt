require File.join('./','build_tools','config.rb')
require 'active_record'
require 'yaml'

namespace :setup do
  task :local do
    config = environment_config("local")
    puts get_config_file_for(NginXTemplate, config["nginx"])
  end

  def environment_config(environmet)
    YAML::load(File.open(File.join(EnvironmentFoler, "#{environmet}.config.yml")))
  end
end
