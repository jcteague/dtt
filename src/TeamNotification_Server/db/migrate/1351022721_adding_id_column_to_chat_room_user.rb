class AddingIdColumnToChatRoomUser < ActiveRecord::Migration
  def self.up
    add_column :chat_room_users, :id, :integer, :primary_key
  end

  def self.down
    remove_column :id
  end
end
