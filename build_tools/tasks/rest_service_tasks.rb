namespace :rest_service do
  task :build_local =>[
    :test,
    :run_migrations
  ]

  task :test do
    puts "running test for rest_service"
  end

  task :run_migrations do
    puts "running migrations"
  end
end

