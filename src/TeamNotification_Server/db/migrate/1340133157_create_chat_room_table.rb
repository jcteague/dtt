class CreateChatRoomTable < ActiveRecord::Migration
  def self.up
	create_table :chat_room do |t|
      	t.column :name, :string, :null => false
        t.column :owner_id, :integer
    end
  end

  def self.down
    drop_table :chat_room
  end
end
