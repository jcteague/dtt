class GithubRepositoriesCollection
    
    constructor: (@github_repositories) ->
    to_json: ->
        format_room = (room) ->
            return { label:room.name, value:room.room_key}
        format_repository = (repository) ->
            return  {label: repository.name, name:repository.name, value:repository.name }        
        
        formated_rooms = (format_room(room) for room in @github_repositories.rooms)
        formated_repositories = (format_repository(repository) for repository in @github_repositories.repositories)
        
        return {
            href: "/github/repositories/#{@github_repositories.access_token}"
            links: [ {"name":"self", "rel": "self", "href": "/github/repositories/#{@github_repositories.access_token}"}, {"name":"home", "rel": "home", "href": "/"} ]
            github_repositories: @github_repositories.repositories
            #rooms:@github_repositories.rooms
            template: 
                href: "/github/repositories/#{@github_repositories.access_token}"
                type: 'repository_selection'
                data: [
                    {name: 'room_key', label: 'Rooms', value: formated_rooms, type: 'dropdownlist'}
                    {name: 'repositories', label: 'Repositories', value: formated_repositories, type: 'dropdownlist', multiple:true}
                    {name: 'owner', label: '', value: @github_repositories.repositories[0].owner.login unless @github_repositories.repositories.length == 0, type: 'hidden'}
                ]
        }

module.exports = GithubRepositoriesCollection
