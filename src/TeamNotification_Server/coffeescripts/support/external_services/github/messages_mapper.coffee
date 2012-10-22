mapper_paths = [
    './mappers/pull_request_comment'
    './mappers/issue_comment'
    './mappers/issue'
    './mappers/push'
    './mappers/comment'
    './mappers/fork'
    './mappers/pull_request'
]

mappers = (require path for path in mapper_paths)

map = (event_obj) ->
  (mapper.map event_obj for mapper in mappers when mapper.can_map event_obj)[0] ? null

module.exports =
  map: map
