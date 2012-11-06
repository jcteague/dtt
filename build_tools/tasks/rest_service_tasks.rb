require File.join('./','build_tools','config.rb')
require 'active_record'
require 'yaml'

namespace :rest_service do
  task :build_local =>[
    :update_packages,
    :copy_environment_files,
    :compile_coffeescript,
    :test_all,
    :dev_environment,
    :migrate
  ]

  task :build_production => [
    :update_packages,
    :copy_environment_files,
    :compile_coffeescript,
    :prod_environment,
    #:test, # This should run on staging env
    :migrate,
    :package,
    :run_production
  ]

  task :deploy => [
    :package_and_deploy
  ]

  task :test_all => [
    :test_environment,
    :reset_database,
    :test
  ]

  task :copy_environment_files do
    section_title "Copying environment configuration"
    yml_source = File.join(BuildToolsRoot, 'environment', "#{ENV['DTT_ENV']}.database.yml")
    server_source = File.join(RestServiceRoot, "coffeescripts", "environment", "#{ENV['DTT_ENV']}.server.coffee")
    client_source = File.join(RestServiceRoot, "coffeescripts", "environment", "#{ENV['DTT_ENV']}.client.coffee")
    sh "cp #{yml_source} #{File.join(BuildToolsRoot, 'database.yml')}"
    sh "cp #{server_source} #{File.join(RestServiceRoot, "coffeescripts", 'config.coffee')}"
    sh "cp #{client_source} #{File.join(RestServiceRoot, "coffeescripts", 'public', 'scripts', 'config.coffee')}"
  end

  task :package_and_deploy do
      Dir.make_temp_dir(RestDeployFolder) do |deploy|
        Dir.glob(File.join(RestServiceRoot, '*')).select{|f| !["coffeescripts", "db", "test"].include? f.split('/').last}.each do |f|
          sh "cp -r #{f} #{deploy}"
        end
        sh "cp #{File.join(RestServiceBuildTools, 'templates', 'nodejitsu_package.json')} package.json"
        sh "node #{File.join(RestServiceBuildTools, "r.js")} -o #{File.join(RestServiceRoot, 'public', 'scripts', 'build.js')}"
        sh "coffee -c app.coffee"
        sh "jitsu deploy"
      end
  end

  task :package do
    section_title "Packaging the application and preparing deploy folder"
    Dir.recreate_dir RestDeployFolder
    Dir.glob(File.join(RestServiceRoot, '*')).select{|f| !["coffeescripts", "db", "test"].include? f.split('/').last}.each do |f|
      sh "cp -r #{f} #{RestDeployFolder}"
    end
    sh "node #{File.join(RestServiceBuildTools, "r.js")} -o #{File.join(RestServiceRoot, 'public', 'scripts', 'build.js')}"
    sh "coffee -c #{File.join(RestDeployFolder, 'app.coffee')}"
  end

  task :run_production do
    section_title "Restarting production server"
    sh "sh #{File.join(RestDeployFolder, 'stop_production.sh')}"
    sh "sh #{File.join(RestDeployFolder, 'start_production.sh')}"
  end

  task :dev_environment do
    section_title "Using dev environment"
    ActiveRecord::Base.establish_connection(db_config["development"])
  end

  task :test_environment do
    section_title "Using test environment"
    ActiveRecord::Base.establish_connection(db_config["test"])
  end

  task :prod_environment do
    section_title "Using production environment"
    ActiveRecord::Base.establish_connection(db_config["production"])
  end

  task :compile_coffeescript do
    section_title "Compiling coffeescript files"
      sh "coffee -o #{RestServiceRoot} -c #{File.join(RestServiceRoot, 'coffeescripts')}"
  end

  task :test do
    current_dir = Dir.pwd
    Dir.chdir(RestServiceRoot)
    sh "mocha"
    Dir.chdir(current_dir)
  end

  task :update_packages do
    section_title "Updating the nodejs packages"
    current_dir = Dir.pwd
    Dir.chdir(RestServiceRoot)
    sh "npm install"
    Dir.chdir(current_dir)
  end

  desc "Migrate the database through scripts in db/migrate"
  task :migrate do
    section_title "Migrating the database #{ActiveRecord::Base.connection.current_database}"
    ActiveRecord::Migrator.migrate(RestServiceMigrations, ENV["VERSION"] ? ENV["VERSION"].to_i : nil )
  end

  desc "Reset the database and migrate"
  task :reset_database => :drop_all_tables do
    section_title "Resetting database #{ActiveRecord::Base.connection.current_database}"
    return unless ActiveRecord::Base.connection.current_database == db_config["test"]["database"]
    ActiveRecord::Migrator.migrate(RestServiceMigrations)
  end

  desc "Truncate all tables in the database and set as create state"
  task :drop_all_tables do
    connection = ActiveRecord::Base.connection
    return unless connection.current_database == db_config["test"]["database"]
    puts "Dropping all tables in #{connection.current_database}"
    tables = connection.tables
    tables.each { |t| connection.execute("DROP TABLE #{t}") }
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

  def section_title(message)
      puts "======================================================"
      puts message.upcase
      puts "======================================================"
  end
end

