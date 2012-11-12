class CreateUserConfirmationKeysTable < ActiveRecord::Migration
  def self.up
	create_table :user_confirmation_keys do |t|
      	t.column :user_id, :integer, :null => false
        t.column :confirmation_key, :string, :null => false
        t.column :created, :date, :null => false, :default => Time.now
        t.column :active, :integer, :default=> 1
    end
  end

  def self.down
    drop_table :user_confirmation_keys
  end
end
