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

  task :build_production => [
    :prod_environment,
    :update_packages,
    :compile_coffeescript,
    #:test, # This should run on staging env
    :migrate,
    :package,
    :run_production
  ]

  task :deploy => [
    #:compile_coffeescript,
    #:test,
    :package_and_deploy
  ]

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
    Dir.recreate_dir RestDeployFolder
    Dir.glob(File.join(RestServiceRoot, '*')).select{|f| !["coffeescripts", "db", "test"].include? f.split('/').last}.each do |f|
      sh "cp -r #{f} #{RestDeployFolder}"
    end
    sh "node #{File.join(RestServiceBuildTools, "r.js")} -o #{File.join(RestServiceRoot, 'public', 'scripts', 'build.js')}"
  end

  task :run_production do
    sh "sudo stop dtt"
    sh "sudo start dtt"
  end

  task :dev_environment do
    ActiveRecord::Base.establish_connection(db_config["development"])
  end

  task :prod_environment do
    ActiveRecord::Base.establish_connection(db_config["production"])
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

