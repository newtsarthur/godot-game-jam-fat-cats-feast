extends Area2D

@export var chao: TileMapLayer
@export var dica1: TileMapLayer
@export var reload: TileMapLayer

var sprite: AnimatedSprite2D
var _players_in_area: int = 0
var can_help: int = 0

func _ready() -> void:
	sprite = $AnimatedSprite2D
	# Conectar os sinais de body_entered e body_exited
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)
	
	# Inicializar as variáveis de visibilidade das camadas
	if chao:
		chao.enabled = false
	if dica1:
		dica1.visible = false
	if reload:
		reload.visible = false

func _on_body_entered(body: Node) -> void:
	if not body.is_in_group("PlayerGroup"):
		return
	
	_players_in_area += 1
	can_help += 1
	print("Entrou: ", body.name, " | Players na área: ", _players_in_area, " | CanHelp: ", can_help)
	
	# Atualizar a visibilidade do botão
	_atualizar_botao()
	
	# Obter o Player e verificar seus clones
	var player := get_node("../Player")  # Assumindo que o Player está no mesmo nível
	if player:
		var clones_current = player.get_clones_current()  # Usando o método para pegar o valor de clones_current
		var clones_max = player.get_clones_max()  # Usando o método para pegar o valor de clones_max
		print("Clones: ", clones_current, "/", clones_max)
		
		# Lógica da dica
		if can_help >= 2 and clones_current == 0:
			print("Posso ajudar!")
			if dica1:
				dica1.visible = true
			if reload:
				reload.visible = false
		else:
			# Esconder a dica se a condição não for atendida
			if dica1:
				dica1.visible = false
			
		# Esconder a dica se o número de clones for igual ao máximo
		if clones_current == clones_max and dica1:
			dica1.visible = false

func _on_body_exited(body: Node) -> void:
	if not body.is_in_group("PlayerGroup"):
		return
	
	_players_in_area = max(0, _players_in_area - 1)
	print("Saiu: ", body.name, " | Players restantes: ", _players_in_area)
	
	if _players_in_area == 0:
		_desativar_botao()
		
		var player := get_node("../Player")
		if player:
			var clones_current = player.get_clones_current()
			var clones_max = player.get_clones_max()
			
			# Mostrar o botão de reload quando apropriado
			if clones_current == clones_max:
				print("Mostrando reload")
				if reload:
					reload.visible = true
			else:
				if reload:
					reload.visible = false

func _atualizar_botao() -> void:
	sprite.play("ButtonClicked")
	if chao:
		chao.enabled = true

func _desativar_botao() -> void:
	sprite.play("ButtonDefault")
	if chao:
		chao.enabled = false
