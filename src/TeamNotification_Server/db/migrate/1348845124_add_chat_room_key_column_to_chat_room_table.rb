class AddChatRoomKeyColumnToChatRoomTable < ActiveRecord::Migration
  def self.up
    add_column :chat_room, :room_key, :string
  end
  
  def self.down
    remove_column :chat_room, :room_key
  end
end
