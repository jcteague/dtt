class CreateUserPasswordResetRequestTable < ActiveRecord::Migration
  def self.up
	create_table :user_password_reset_request do |t|
      	t.column :user_id, :integer, :null => false
        t.column :reset_key, :string, :null => false
        t.column :created, :date, :null => false, :default => Time.now
        t.column :active, :integer, :default=> 1
    end
  end

  def self.down
    drop_table :user_password_reset_request
  end
end
