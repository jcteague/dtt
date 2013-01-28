class BitbucketRepositoriesCollection
    
    constructor: (@repositories) ->
    to_json: ->
        format_room = (room) ->
            return { label:room.name, value:room.room_key}
        format_repository = (repository) ->
            return  {label: repository.name, name:repository.name, value:repository.url }        
        
        formated_rooms = (format_room(room) for room in @repositories.rooms)
        formated_repositories = (format_repository(repository) for repository in @repositories.repositories)
        return {
            href: "/bitbucket/repositories/#{@repositories.oauth_token}?oauth_token_secret=#{@repositories.oauth_token_secret}"
            links: [ {"name":"self", "rel": "self", "href": "/bitbucket/repositories/#{@repositories.oauth_token}"}, {"name":"home", "rel": "home", "href": "/"} ]
            bitbucket_repositories: @repositories.repositories
            #rooms:@repositories.rooms
            template: 
                href: "/bitbucket/repositories/#{@repositories.oauth_token}?oauth_token_secret=#{@repositories.oauth_token_secret}"
                type: 'repository_selection'
                data: [
                    {name: 'room_key', label: 'Rooms', value: formated_rooms, type: 'dropdownlist'}
                    {name: 'repositories', label: 'Repositories', value: formated_repositories, type: 'dropdownlist', multiple:true}
                    {name: 'owner', label: '', value: @repositories.repositories[0].owner.login, type: 'hidden'}
                ]
        }

module.exports = BitbucketRepositoriesCollection
