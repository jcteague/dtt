require File.join('./','build_tools','config.rb')
require 'active_record'
require 'yaml'

namespace :rest_service do
  task :build_local =>[
    :dev_environment,
    :test,
    :migrate
  ]

  task :dev_environment do
    ActiveRecord::Base.establish_connection(db_config["development"])
  end

  task :test do
    puts "running test for rest_service"
  end

  desc "Migrate the database through scripts in db/migrate"
  task :migrate do
    ActiveRecord::Migrator.migrate(RestServiceMigrations, ENV["VERSION"] ? ENV["VERSION"].to_i : nil )
  end

  def db_config
    YAML::load(File.open(RestServiceDatabaseConfig))
  end
end

