class CreateChatRoomMembersTable < ActiveRecord::Migration
  def self.up
	create_table :chat_room_members do |t|
      	t.column :room_id, :integer, :null => false
      	t.column :user_id, :integer, :null => false
    end
  end

  def self.down
    drop_table :chat_room_members
  end
end
