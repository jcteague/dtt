class CreateChatRoomTable < ActiveRecord::Migration
  def self.up
	create_table :chat_room do |t|
		t.column :id, :int, :autoincrement
      		t.column :name, :string, :null => false
     	end
  end

  def self.down
    drop_table :chat_room
  end
end
