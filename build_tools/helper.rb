def all_tasks
  FileList.new(File.join(File.dirname(File.expand_path(__FILE__)),'tasks','*_tasks.rb'))
end
