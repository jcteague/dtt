require File.join('./','build_tools','helper.rb')
all_tasks.each {|f| require f}

task :build_local => [
  'rest_service:build_local'
]

task :build_release => [:build_local] do
end
