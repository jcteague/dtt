require 'fileutils'
require 'erb'
require 'ostruct'

def all_tasks
  FileList.new(File.join(File.dirname(File.expand_path(__FILE__)),'tasks','*_tasks.rb'))
end

def get_dirname_for(file)
  File.dirname(File.expand_path(file))
end

def generate_timestamp
  Time.now.to_i
end

def convert_xml_file_to_hash(file_path)
  require 'crack'
  Crack::XML.parse(File.read(file_path))
end

def write_hash_to_json_file(file_path, hash)
  require 'crack'
  require 'json'
  File.open(file_path, "w") do |f|
    f.write(hash.to_json)
  end
end

def get_config_file_for(file_path, config)
  template = File.read(file_path)
  ErbTemplate.new(config).render(template)
end

class ErbTemplate < OpenStruct
  def render(template)
    ERB.new(template).result(binding)
  end
end

class String
  def to_camel_case
    words = self.downcase.split("_")
    words.map {|w| w.capitalize }.join
  end
end

class Dir
  def self.make_temp_dir(path, &block)
    current_dir = Dir.pwd
    self.mkdir path, 0700
    Dir.chdir(path)
    yield(path)
    Dir.chdir(current_dir)
    FileUtils.rm_rf path
  end

  def self.recreate_dir(path)
    FileUtils.rm_rf path
    self.mkdir path
  end
end
