class CreateChatroomMessageTable < ActiveRecord::Migration
  def self.up
    create_table :chat_room_messages do |t| 
        t.column :body,    :string,  :null => false
        t.column :date,    :timestamp,  :null => false
      	t.column :room_id, :integer, :null => false
      	t.column :user_id, :integer, :null => false
      	end
  end

  def self.down
    drop_table :chat_room_messages
  end
end
