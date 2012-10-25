class AddingIdColumnToChatRoomUser < ActiveRecord::Migration
  def self.up
    add_column :chat_room_users, :id, :primary_key
  end

  def self.down
    remove_column :chat_room_users, :id
  end
end
