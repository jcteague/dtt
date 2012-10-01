class GithubRepositoriesCollection
    
    constructor: (@github_repositories) ->
    to_json: ->
        other_links = []
       
        return {
            self = {"name":"self", "rel": "Repositories", "href": "/github/repositories/#{@github_repositories.access_token}"}
            github_repositories: github_repositories
        }

module.exports = GithubRepositoriesCollection
