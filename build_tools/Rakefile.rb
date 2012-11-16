require "rubygems"
require "bundler/setup"

require File.join('./','build_tools','helper.rb')
require 'erb'

all_tasks.each {|f| require f}

task :build_local => [
  'rest_service:build_local'
]

task :build_release => [
  'rest_service:build_production'
]

task :build_vs_package => [
  'visual_studio:build'
]

task :setup_local => [
  'setup:local'
]

task :setup_staging => [
  'setup:staging'
]

task :setup_production => [
  'setup:production'
]

task :create_migration, :migration_name do |t, args|
  migration_name = args[:migration_name]
  filename = "#{generate_timestamp}_#{migration_name}.rb"
  template = ERB.new(File.open(File.join(".","build_tools","templates","empty_migration.rb.erb")).read)
  migration_class_name = migration_name.to_camel_case
  migration_content = template.result(binding)
  full_file_path = File.join(RestServiceMigrations,filename)
  File.open(full_file_path,'w') {|f| f.write(migration_content)}
  puts "new migration created at #{full_file_path}"
end

task :rollback_db => [
  'rest_service:rollback_db'
]

task :deploy => [
  'rest_service:deploy'
]
