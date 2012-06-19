class CreateChatRoomUsersTable < ActiveRecord::Migration
  def self.up
	create_table :chat_room_users do |t|
      	t.column :chat_room_id, :integer, :null => false
      	t.column :user_id, :integer, :null => false
    end
  end

  def self.down
    drop_table :chat_room_users
  end
end
