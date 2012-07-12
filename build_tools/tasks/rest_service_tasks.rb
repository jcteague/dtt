require File.join('./','build_tools','config.rb')
require 'active_record'
require 'yaml'

namespace :rest_service do
  task :build_local =>[
    :dev_environment,
    :update_packages,
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

  multitask :integration_test => [:run_test_server] do
  end

  task :test do
    current_dir = Dir.pwd
    Dir.chdir(RestServiceRoot)
    sh "mocha"
    Dir.chdir(current_dir)
  end

  task :update_packages do
    current_dir = Dir.pwd
    Dir.chdir(RestServiceRoot)
    sh "npm install"
    Dir.chdir(current_dir)
  end

  desc "Migrate the database through scripts in db/migrate"
  task :migrate do
    ActiveRecord::Migrator.migrate(RestServiceMigrations, ENV["VERSION"] ? ENV["VERSION"].to_i : nil )
  end

  task :rollback_db => [:dev_environment] do
    ActiveRecord::Migrator.rollback(RestServiceMigrations)
  end

  desc "Start the server in test environment"
  task :run_test_server do
    sh "sh run_test_server.sh"
  end

  def db_config
    YAML::load(File.open(RestServiceDatabaseConfig))
  end
end

