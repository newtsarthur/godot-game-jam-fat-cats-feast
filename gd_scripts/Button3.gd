extends Area2D

@export var chao: TileMapLayer  # O chão que será ativado/desativado

var _players_in_area: int = 0
var can_help: int = 0
var _sprite: AnimatedSprite2D

func _ready() -> void:
	_sprite = $AnimatedSprite2D
	
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)

	if chao:
		chao.enabled = false


func _on_body_entered(body: Node) -> void:
	if body.is_in_group("PlayerGroup"):
		_players_in_area += 1
		_ativar_botao()
		print("Entrou: %s | Total na área: %d" % [body.name, _players_in_area])
		var player = get_node_or_null("../Player")


func _on_body_exited(body: Node) -> void:
	if body.is_in_group("PlayerGroup"):
		_players_in_area = max(0, _players_in_area - 1)

		if _players_in_area == 0:
			_desativar_botao()
			print("Saiu: %s | Botão desativado (0 players na área)" % body.name)
		else:
			print("Saiu: %s | Ainda há %d players na área" % [body.name, _players_in_area])


func _ativar_botao() -> void:
	_sprite.play("ButtonClicked")
	if chao:
		chao.enabled = true
		# print("Funciona")


func _desativar_botao() -> void:
	_sprite.play("ButtonDefault")
	if chao:
		chao.enabled = false
		# print("Desativado")
