class AddingOwnerIdColumnToChatRoomsTable < ActiveRecord::Migration
  def self.up
    add_column :chat_room, :owner_id, :integer
  end

  def self.down
    remove_column :chat_room, :owner_id
  end
end
