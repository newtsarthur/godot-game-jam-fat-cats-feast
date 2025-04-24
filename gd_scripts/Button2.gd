extends Area2D

@export var thorn: NodePath  # Assuming Thorn is a Node, adjust type as needed
var _thorn_node: Node  # Actual reference to the thorn node
var _sprite: AnimatedSprite2D
var _players_in_area: int = 0
var mouse_death: int = 0

func _ready() -> void:
	_sprite = $AnimatedSprite2D
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)
	
	# Get the actual thorn node if path was provided
	if thorn:
		_thorn_node = get_node(thorn)

func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("PlayerGroup"):
		_players_in_area += 1
		if _players_in_area == 1:
			_ativar_botao()

func _on_body_exited(body: Node2D) -> void:
	if body.is_in_group("PlayerGroup"):
		_players_in_area = max(0, _players_in_area - 1)
		if _players_in_area == 0:
			_desativar_botao()

func _ativar_botao() -> void:
	_sprite.play("ButtonClicked")
	if _thorn_node and _thorn_node.has_method("start_falling"):
		_thorn_node.start_falling()

func _desativar_botao() -> void:
	_sprite.play("ButtonDefault")
	# Doesn't do anything with thorn here
