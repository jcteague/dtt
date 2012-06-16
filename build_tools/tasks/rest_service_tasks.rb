require File.join('./','build_tools','config.rb')
require 'active_record'
require 'yaml'

namespace :rest_service do
  task :build_local =>[
    :dev_environment,
    :compile_coffeescript,
    :test,
    :migrate
  ]

  task :dev_environment do
    ActiveRecord::Base.establish_connection(db_config["development"])
  end

  task :compile_coffeescript do
      sh "coffee -o #{RestServiceRoot} -c #{File.join(RestServiceRoot, 'coffeescripts')}"
  end

  task :test do
    current_dir = Dir.pwd
    Dir.chdir(RestServiceRoot)
    sh "mocha"
    Dir.chdir(current_dir)
  end

  desc "Migrate the database through scripts in db/migrate"
  task :migrate do
    ActiveRecord::Migrator.migrate(RestServiceMigrations, ENV["VERSION"] ? ENV["VERSION"].to_i : nil )
  end

  def db_config
    YAML::load(File.open(RestServiceDatabaseConfig))
  end
end

