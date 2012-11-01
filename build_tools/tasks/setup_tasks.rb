require File.join('./','build_tools','config.rb')
require 'active_record'
require 'yaml'

namespace :setup do
  task :local do
    set_nginx_for "local"
  end

  task :production do
    set_nginx_for "production"
  end

  def set_nginx_for(environment)
    config = environment_config(environment)
    nginx_config = get_config_file_for(NginXTemplate, config["nginx"])
    write_config_file(nginx_config)
    sh "cp #{File.join(TemplatesFolder, 'yacketyapp.com')} #{config['nginx']['sites_available_path']}"
    sh "ln -s #{File.join(config['nginx']['sites_available_path'], 'yacketyapp.com')} #{File.join(config['nginx']['sites_enabled_path'], 'yacketyapp.com')}"
  end

  def environment_config(environmet)
    YAML::load(File.open(File.join(EnvironmentFoler, "#{environmet}.config.yml")))
  end

  def write_config_file(config_file)
    File.open(File.join(TemplatesFolder, 'yacketyapp.com'), "w") do |f|
      f.write(config_file)
    end
  end
end
