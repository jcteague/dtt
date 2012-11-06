require File.join('./','build_tools','config.rb')
require 'active_record'
require 'yaml'

namespace :setup do
  task :local do
    set_stunnel_for "local"
    set_varnish_for "local"
    set_nginx_for "local"
  end

  task :staging do
    set_stunnel_for "staging"
    set_varnish_for "staging"
    set_nginx_for "staging"
  end

  task :production do
    set_stunnel_for "production"
    set_varnish_for "production"
    set_nginx_for "production"
  end

  def set_nginx_for(environment)
    config = environment_config(environment)
    nginx_config = get_config_file_for(NginXTemplate, config["nginx"])
    file_name = 'yacketyapp.com'
    write_config_file(nginx_config, file_name)
    sh "cp #{File.join(TemplatesFolder, file_name)} #{config['nginx']['sites_available_path']}"
    sh "ln -s #{File.join(config['nginx']['sites_available_path'], file_name)} #{File.join(config['nginx']['sites_enabled_path'], file_name)}"
    # To unconfigure the the default port
    sh "rm #{File.join(config['nginx']['sites_enabled_path'], 'default')}"
  end

  def set_stunnel_for(environment)
    config = environment_config(environment)
    stunnel_config = get_config_file_for(STunnelTemplate, config["stunnel"])
    file_name = 'https-proxy.conf'
    write_config_file(stunnel_config, file_name)
    sh "cp #{File.join(TemplatesFolder, file_name)} #{config['stunnel']['config_path']}"
    sh "cp #{STunnelDefault} #{config['stunnel']['default']}"
  end

  def set_varnish_for(environment)
    config = environment_config(environment)
    varnish_config = get_config_file_for(VarnishTemplate, config["varnish"])
    file_name = 'default.vcl'
    write_config_file(varnish_config, file_name)
    sh "cp #{File.join(TemplatesFolder, file_name)} #{config['varnish']['config_path']}"
    sh "cp #{VarnishDefault} #{config['varnish']['default']}"
  end

  def environment_config(environment)
    YAML::load(File.open(File.join(EnvironmentFoler, "#{environment}.config.yml")))
  end

  def write_config_file(config_file, name)
    File.open(File.join(TemplatesFolder, name), "w") do |f|
      f.write(config_file)
    end
  end

  def replace_config_file_value(file_path, pattern, substitution)
    puts file_path, pattern, substitution
    new_config = File.read(file_path).gsub(pattern, substitution)
    File.open(file_path, "w") {|file| file.puts new_config}
  end
end
