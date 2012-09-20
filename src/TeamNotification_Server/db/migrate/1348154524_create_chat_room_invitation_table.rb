class CreateChatRoomInvitationTable < ActiveRecord::Migration
  def self.up
    create_table :chat_room_invitation do |t| 
        t.column :chat_room_id,    :integer,  :null => false
        t.column :email,    :string,  :null => false
        t.column :date,    :timestamp,  :null => false, :default => Time.now
      	t.column :accepted, :integer, :null => false, :default => 0
      	t.column :user_id, :integer, :null => false
    end
  end

  def self.down
    drop_table :chat_room_invitation
  end
end
