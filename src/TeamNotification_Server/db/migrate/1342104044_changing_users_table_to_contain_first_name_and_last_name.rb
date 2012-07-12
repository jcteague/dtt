class ChangingUsersTableToContainFirstNameAndLastName < ActiveRecord::Migration
  def self.up
    change_table :users do |t|
      t.rename :name, :first_name
    end

    add_column :users, :last_name, :string

  end

  def self.down
    remove_column :users, :last_name
    change_table :users do |t|
      t.rename :first_name, :name
    end
  end
end
