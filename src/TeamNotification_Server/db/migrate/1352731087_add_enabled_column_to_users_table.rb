class AddEnabledColumnToUsersTable < ActiveRecord::Migration
  def self.up
	add_column :users, :enabled, :integer, :default => 0, :null => false 
  end

  def self.down
	remove_column :users, :enabled
  end
end
