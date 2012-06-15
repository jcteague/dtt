def all_tasks
  FileList.new(File.join(File.dirname(File.expand_path(__FILE__)),'tasks','*_tasks.rb'))
end

def get_dirname_for(file)
  File.dirname(File.expand_path(file))
end

def generate_timestamp
  Time.now.getutc.to_s.gsub(/[^\d]/,'')
end

class String
  def to_camel_case
    words = self.downcase.split("_")
    words.map {|w| w.capitalize }.join
  end
end
