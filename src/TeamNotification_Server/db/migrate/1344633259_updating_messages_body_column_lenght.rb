class UpdatingMessagesBodyColumnLenght < ActiveRecord::Migration
  def self.up
    change_column :chat_room_messages, :body, :text
  end

  def self.down
    change_column :chat_room_messages, :body, :string
  end
end
