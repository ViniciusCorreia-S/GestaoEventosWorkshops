const apiBase = "/api";
let token = localStorage.getItem("token") || "";
let participantes = [];
let eventos = [];
let workshops = [];
let inscricoes = [];
let organizadores = [];
let exclusaoAtual = null;
let perfilAtual = localStorage.getItem("perfil") || "";
let participanteAtual = JSON.parse(localStorage.getItem("participanteAtual") || "null");

const elementos = {
    loginPage: document.getElementById("loginPage"),
    homePage: document.getElementById("homePage"),
    participantPage: document.getElementById("participantPage"),
    loginForm: document.getElementById("loginForm"),
    cadastroContaForm: document.getElementById("cadastroContaForm"),
    contaNome: document.getElementById("contaNome"),
    contaEmail: document.getElementById("contaEmail"),
    contaCodigo: document.getElementById("contaCodigo"),
    contaDataNascimento: document.getElementById("contaDataNascimento"),
    contaAceiteLgpd: document.getElementById("contaAceiteLgpd"),
    usuario: document.getElementById("usuario"),
    senha: document.getElementById("senha"),
    loginAceiteLgpd: document.getElementById("loginAceiteLgpd"),
    btnLogin: document.getElementById("btnLogin"),
    btnLogout: document.getElementById("btnLogout"),
    btnLogoutParticipante: document.getElementById("btnLogoutParticipante"),
    loginStatus: document.getElementById("loginStatus"),
    mensagem: document.getElementById("mensagem"),
    form: document.getElementById("participanteForm"),
    participanteId: document.getElementById("participanteId"),
    nome: document.getElementById("nome"),
    email: document.getElementById("email"),
    inscricao: document.getElementById("inscricao"),
    dataNascimento: document.getElementById("dataNascimento"),
    ativo: document.getElementById("ativo"),
    tabela: document.getElementById("participantesTabela"),
    btnAtualizar: document.getElementById("btnAtualizar"),
    abaOrganizadoresItem: document.getElementById("abaOrganizadoresItem"),
    btnCancelar: document.getElementById("btnCancelar"),
    buscaParticipante: document.getElementById("buscaParticipante"),
    filtroEvento: document.getElementById("filtroEvento"),
    filtroStatus: document.getElementById("filtroStatus"),
    dashTotalParticipantes: document.getElementById("dashTotalParticipantes"),
    dashParticipantesAtivos: document.getElementById("dashParticipantesAtivos"),
    dashTotalEventos: document.getElementById("dashTotalEventos"),
    dashMediaParticipantesEvento: document.getElementById("dashMediaParticipantesEvento"),
    dashTotalWorkshops: document.getElementById("dashTotalWorkshops"),
    dashCargaHoraria: document.getElementById("dashCargaHoraria"),
    dashTotalInscricoes: document.getElementById("dashTotalInscricoes"),
    dashUltimaInscricao: document.getElementById("dashUltimaInscricao"),
    dashParticipantesPorEvento: document.getElementById("dashParticipantesPorEvento"),
    dashResumoRelatorios: document.getElementById("dashResumoRelatorios"),
    dashUltimasInscricoes: document.getElementById("dashUltimasInscricoes"),
    eventoForm: document.getElementById("eventoForm"),
    eventoCadastroId: document.getElementById("eventoCadastroId"),
    eventoNome: document.getElementById("eventoNome"),
    eventoCodigo: document.getElementById("eventoCodigo"),
    eventoLocal: document.getElementById("eventoLocal"),
    eventoDataInicio: document.getElementById("eventoDataInicio"),
    eventoDataFim: document.getElementById("eventoDataFim"),
    eventoOrganizadorId: document.getElementById("eventoOrganizadorId"),
    eventosTabela: document.getElementById("eventosTabela"),
    btnCancelarEvento: document.getElementById("btnCancelarEvento"),
    workshopForm: document.getElementById("workshopForm"),
    workshopId: document.getElementById("workshopId"),
    workshopNome: document.getElementById("workshopNome"),
    workshopCodigo: document.getElementById("workshopCodigo"),
    workshopCargaHoraria: document.getElementById("workshopCargaHoraria"),
    workshopEventoId: document.getElementById("workshopEventoId"),
    workshopsTabela: document.getElementById("workshopsTabela"),
    btnCancelarWorkshop: document.getElementById("btnCancelarWorkshop"),
    inscricaoForm: document.getElementById("inscricaoForm"),
    inscricaoParticipanteId: document.getElementById("inscricaoParticipanteId"),
    inscricaoWorkshopId: document.getElementById("inscricaoWorkshopId"),
    inscricoesTabela: document.getElementById("inscricoesTabela"),
    modalExclusao: document.getElementById("modalExclusao"),
    exclusaoTexto: document.getElementById("exclusaoTexto"),
    registroExclusaoNome: document.getElementById("registroExclusaoNome"),
    btnConfirmarExclusao: document.getElementById("btnConfirmarExclusao"),
    organizadorForm: document.getElementById("organizadorForm"),
    organizadorNome: document.getElementById("organizadorNome"),
    organizadorEmail: document.getElementById("organizadorEmail"),
    organizadorSenha: document.getElementById("organizadorSenha"),
    organizadoresTabela: document.getElementById("organizadoresTabela"),
    modalLoginErro: document.getElementById("modalLoginErro"),
    loginErroMensagem: document.getElementById("loginErroMensagem"),
    participanteHeaderNome: document.getElementById("participanteHeaderNome"),
    participanteTitulo: document.getElementById("participanteTitulo"),
    participanteMetricEventos: document.getElementById("participanteMetricEventos"),
    participanteMetricWorkshops: document.getElementById("participanteMetricWorkshops"),
    participanteMetricHistorico: document.getElementById("participanteMetricHistorico"),
    participanteBusca: document.getElementById("participanteBusca"),
    participanteFiltroTipo: document.getElementById("participanteFiltroTipo"),
    participanteFiltroStatus: document.getElementById("participanteFiltroStatus"),
    participanteCards: document.getElementById("participanteCards"),
    participanteHistoricoHoras: document.getElementById("participanteHistoricoHoras"),
    participanteHistoricoTabela: document.getElementById("participanteHistoricoTabela"),
    mensagemParticipante: document.getElementById("mensagemParticipante"),
    nomedapagina: document.getElementById("nomedapagina"),
    olhoSenha: document.getElementById('olhoSenha'),
};

const modalExclusao = new bootstrap.Modal(elementos.modalExclusao);
const modalLoginErro = new bootstrap.Modal(elementos.modalLoginErro);

function headersJson() {
    const headers = { "Content-Type": "application/json" };
    if (token) headers.Authorization = `Bearer ${token}`;
    return headers;
}

async function requisicao(url, opcoes = {}) {
    let resposta;

    try {
        resposta = await fetch(url, { ...opcoes, headers: { ...headersJson(), ...(opcoes.headers || {}) } });
    } catch {
        throw new Error("Não foi possível conectar à API. Verifique se o servidor do sistema está em execução e se o banco de dados está disponível.");
    }

    const texto = await resposta.text();
    let corpo = {};

    try {
        corpo = texto ? JSON.parse(texto) : {};
    } catch {
        corpo = { mensagem: texto };
    }

    if (!resposta.ok) throw new Error(obterMensagemErro(resposta, corpo, texto));
    return corpo;
}

function obterMensagemErro(resposta, corpo, textoResposta) {
    if (corpo?.mensagem) return corpo.mensagem;

    const errosValidacao = obterErrosValidacao(corpo);
    if (errosValidacao.length) {
        return `Confira os campos preenchidos: ${errosValidacao.join(" ")}`;
    }

    if (resposta.status === 400) {
        return "A solicitação não pôde ser processada. Revise os campos obrigatórios, formatos e valores informados.";
    }

    if (resposta.status === 401) {
        return "Sua sessão expirou ou o login não foi aceito. Entre novamente com usuário e senha válidos.";
    }

    if (resposta.status === 403) {
        return "Seu usuário não tem permissão para realizar esta ação. Use uma conta de administrador ou solicite acesso.";
    }

    if (resposta.status === 404) {
        return "O registro solicitado não foi encontrado. Atualize os dados da tela e tente novamente.";
    }

    if (resposta.status === 409) {
        return "Não foi possível concluir porque já existe um registro conflitante com essas informações.";
    }

    if (resposta.status >= 500 || textoResposta.includes("MySql") || textoResposta.includes("Exception")) {
        return "O sistema encontrou um problema interno ao acessar os dados. Verifique se o MySQL está configurado, ativo e acessível.";
    }

    return textoResposta.slice(0, 220) || "Não foi possível concluir a operação. Revise os dados e tente novamente.";
}

function obterErrosValidacao(corpo) {
    if (!corpo?.errors || typeof corpo.errors !== "object") return [];

    const nomesCampos = {
        nome: "Nome",
        email: "E-mail",
        codigoInscricao: "Código de inscrição",
        dataNascimento: "Data de nascimento",
        codigo: "Código",
        local: "Local",
        dataInicio: "Data de início",
        dataFim: "Data de fim",
        cargaHoraria: "Carga horária",
        eventoId: "Evento",
        participanteId: "Participante",
        workshopId: "Workshop",
        usuario: "Usuário",
        senha: "Senha"
    };

    return Object.entries(corpo.errors).flatMap(([campo, mensagens]) => {
        const nomeCampo = nomesCampos[campo.split(".").pop()] || campo;
        const lista = Array.isArray(mensagens) ? mensagens : [String(mensagens)];
        return lista.map(mensagem => `${nomeCampo}: ${mensagem}`);
    });
}
function escaparHtml(valor) {
    return String(valor ?? "").replaceAll("&", "&amp;").replaceAll("<", "&lt;").replaceAll(">", "&gt;").replaceAll('"', "&quot;").replaceAll("'", "&#039;");
}

function normalizarTexto(valor) {
    return String(valor ?? "").toLowerCase().trim();
}

function formatarData(valor) {
    if (!valor) return "";
    return new Date(valor).toLocaleDateString("pt-BR");
}

function formatarDataSimples(valor) {
    if (!valor) return "";
    const [ano, mes, dia] = valor.split("-");
    return `${dia}/${mes}/${ano}`;
}

function definirCarregando(botao, carregando, texto = "Carregando...") {
    if (!botao.dataset.textoOriginal) botao.dataset.textoOriginal = botao.textContent;
    botao.disabled = carregando;
    botao.textContent = carregando ? texto : botao.dataset.textoOriginal;
}

function mostrarMensagem(texto, tipo = "success") {
    const alvo = elementos.participantPage.classList.contains("d-none") ? elementos.mensagem : elementos.mensagemParticipante;
    alvo.className = `app-alert app-alert-${tipo}`;
    const configuracoes = {
        success: { titulo: "Sucesso", simbolo: "✓" },
        danger: { titulo: "Atenção", simbolo: "!" },
        warning: { titulo: "Atenção", simbolo: "!" },
        info: { titulo: "Aviso", simbolo: "i" }
    };
    const alerta = configuracoes[tipo] || configuracoes.info;
    alvo.innerHTML = `<div class="app-alert-icon"><span>${alerta.simbolo}</span></div><div class="app-alert-content"><strong>${alerta.titulo}</strong><span>${escaparHtml(texto)}</span></div><button class="app-alert-close" type="button" aria-label="Fechar alerta">X</button>`;
    alvo.querySelector("button").addEventListener("click", esconderMensagem);
}
function esconderMensagem() {
    elementos.mensagem.className = "app-alert d-none";
    elementos.mensagem.textContent = "";
    elementos.mensagemParticipante.className = "app-alert d-none";
    elementos.mensagemParticipante.textContent = "";
}

function mostrarTelaLogin() {
    elementos.loginPage.classList.remove("d-none");
    elementos.homePage.classList.add("d-none");
    elementos.participantPage.classList.add("d-none");
    elementos.loginStatus.textContent = "Equipe: admin/123456. Organizador: e-mail/senha cadastrados. Participante: e-mail/codigo de inscricao.";
}

function mostrarHome() {
    elementos.loginPage.classList.add("d-none");
    elementos.homePage.classList.remove("d-none");
    elementos.participantPage.classList.add("d-none");
}

function mostrarAreaParticipante() {
    elementos.loginPage.classList.add("d-none");
    elementos.homePage.classList.add("d-none");
    elementos.participantPage.classList.remove("d-none");
}

async function login(evento) {
    evento.preventDefault();
    definirCarregando(elementos.btnLogin, true, "Entrando...");
    try {
        const resultado = await requisicao(`${apiBase}/auth/login`, {
            method: "POST",
            body: JSON.stringify({
                usuario: elementos.usuario.value,
                senha: elementos.senha.value,
                aceiteTermosLgpd: elementos.loginAceiteLgpd.checked
            })
        });
        token = resultado.token;
        perfilAtual = resultado.perfil || "";
        participanteAtual = resultado.participante || null;
        localStorage.setItem("token", token);
        localStorage.setItem("perfil", perfilAtual);
        if (participanteAtual) {
            localStorage.setItem("participanteAtual", JSON.stringify(participanteAtual));
            await iniciarAreaParticipante();
        } else {
            localStorage.removeItem("participanteAtual");
            await iniciarHome();
        }
    } catch (erro) {
        token = "";
        perfilAtual = "";
        participanteAtual = null;
        localStorage.removeItem("token");
        localStorage.removeItem("perfil");
        localStorage.removeItem("participanteAtual");
        elementos.loginErroMensagem.textContent = erro.message || "Usuário ou senha inválidos.";
        modalLoginErro.show();
    } finally {
        definirCarregando(elementos.btnLogin, false);
    }
}

function logout() {
    token = "";
    perfilAtual = "";
    participanteAtual = null;
    localStorage.removeItem("token");
    localStorage.removeItem("perfil");
    localStorage.removeItem("participanteAtual");
    mostrarTelaLogin();
}

async function cadastrarContaParticipante(evento) {
    evento.preventDefault();
    const botao = elementos.cadastroContaForm.querySelector("button[type='submit']");
    definirCarregando(botao, true, "Cadastrando...");
    try {
        const corpo = {
            nome: elementos.contaNome.value,
            email: elementos.contaEmail.value,
            codigoInscricao: elementos.contaCodigo.value,
            dataNascimento: elementos.contaDataNascimento.value,
            aceiteTermosLgpd: elementos.contaAceiteLgpd.checked
        };
        await requisicao(`${apiBase}/participantes`, {
            method: "POST",
            body: JSON.stringify(corpo)
        });

        elementos.usuario.value = elementos.contaEmail.value;
        elementos.senha.value = elementos.contaCodigo.value;
        elementos.loginAceiteLgpd.checked = true;
        await login(new Event("submit"));
    } catch (erro) {
        elementos.loginErroMensagem.textContent = erro.message || "Nao foi possivel cadastrar sua conta.";
        modalLoginErro.show();
    } finally {
        definirCarregando(botao, false);
    }
}

async function iniciarHome() {
    mostrarHome();
    esconderMensagem();
    await carregarDadosHome();
}

async function iniciarAreaParticipante() {
    mostrarAreaParticipante();
    esconderMensagem();
    await carregarDadosParticipante();
}

async function carregarDadosHome() {
    configurarPermissoesInterface();
    await carregarOrganizadores();
    await carregarEventos();
    await carregarParticipantes();
    await carregarWorkshops();
    await carregarInscricoes();
    renderizarDashboard();
}

function configurarPermissoesInterface() {
    const admin = perfilAtual === "Administrador";
    elementos.abaOrganizadoresItem.classList.toggle("d-none", !admin);
	elementos.nomedapagina.textContent = admin ? "Área do Administrador" : "Área do Organizador";
    [elementos.eventoForm, elementos.workshopForm, elementos.inscricaoForm].forEach(form => {
        form.closest(".col-12").classList.toggle("d-none", !admin);
    });
}

async function carregarOrganizadores() {
    if (perfilAtual !== "Administrador") {
        organizadores = [];
        preencherSelectOrganizadores();
        return;
    }

    const resultado = await requisicao(`${apiBase}/organizadores`);
    organizadores = resultado.dados;
    preencherSelectOrganizadores();
    renderizarTabelaOrganizadores();
}

function preencherSelectOrganizadores() {
    const opcoes = organizadores.map(organizador => `<option value="${organizador.id}">${escaparHtml(organizador.nome)}</option>`).join("");
    elementos.eventoOrganizadorId.innerHTML = `<option value="">Sem responsável</option>${opcoes}`;
}

async function carregarEventos() {
    const resultado = await requisicao(`${apiBase}/eventos`);
    eventos = resultado.dados;
    preencherSelectsEventos();
    renderizarTabelaEventos();
}

function preencherSelectsEventos() {
    const opcoes = eventos.map(evento => `<option value="${evento.id}">${escaparHtml(evento.nome)}</option>`).join("");
    elementos.workshopEventoId.innerHTML = `<option value="" disabled selected>Selecione um evento</option>${opcoes}`;
    elementos.filtroEvento.innerHTML = `<option value="">Todos os eventos</option>${opcoes}`;
}

function renderizarTabelaEventos() {
    if (!eventos.length) {
        elementos.eventosTabela.innerHTML = `<tr><td colspan="5" class="text-center py-4">Nenhum evento cadastrado.</td></tr>`;
        return;
    }
    elementos.eventosTabela.innerHTML = eventos.map(evento => `<tr><td>${escaparHtml(evento.codigo)}</td><td>${escaparHtml(evento.nome)}<div class="small text-muted">${escaparHtml(evento.organizadorNome || "Sem responsável")}</div></td><td>${escaparHtml(evento.local)}</td><td>${formatarDataSimples(evento.dataInicio)} a ${formatarDataSimples(evento.dataFim)}</td><td class="text-end">${perfilAtual === "Administrador" ? `<button class="btn btn-outline-secondary btn-sm" onclick="editarEvento(${evento.id})">Editar</button> <button class="btn btn-outline-danger btn-sm" onclick="abrirExclusaoEvento(${evento.id})">Excluir</button>` : `<span class="text-muted">Somente leitura</span>`}</td></tr>`).join("");
}

function editarEvento(id) {
    const evento = eventos.find(item => item.id === id);
    if (!evento) return;
    elementos.eventoCadastroId.value = evento.id;
    elementos.eventoNome.value = evento.nome;
    elementos.eventoCodigo.value = evento.codigo;
    elementos.eventoLocal.value = evento.local;
    elementos.eventoDataInicio.value = evento.dataInicio;
    elementos.eventoDataFim.value = evento.dataFim;
    elementos.eventoOrganizadorId.value = evento.organizadorId || "";
}

function limparFormularioEvento() {
    elementos.eventoForm.reset();
    elementos.eventoCadastroId.value = "";
}

async function salvarEvento(evento) {
    evento.preventDefault();
    const id = elementos.eventoCadastroId.value;
    const corpo = {
        nome: elementos.eventoNome.value,
        codigo: elementos.eventoCodigo.value,
        local: elementos.eventoLocal.value,
        dataInicio: elementos.eventoDataInicio.value,
        dataFim: elementos.eventoDataFim.value,
        organizadorId: elementos.eventoOrganizadorId.value ? Number(elementos.eventoOrganizadorId.value) : null
    };
    await salvar(`${apiBase}/eventos${id ? `/${id}` : ""}`, id ? "PUT" : "POST", corpo, id ? "Evento atualizado com sucesso." : "Evento cadastrado com sucesso.", async () => {
        limparFormularioEvento();
        await carregarEventos();
        await carregarWorkshops();
    }, elementos.eventoForm);
}

function renderizarTabelaOrganizadores() {
    if (!organizadores.length) {
        elementos.organizadoresTabela.innerHTML = `<tr><td colspan="3" class="text-center py-4">Nenhum organizador cadastrado.</td></tr>`;
        return;
    }

    elementos.organizadoresTabela.innerHTML = organizadores.map(organizador => `<tr><td>${escaparHtml(organizador.nome)}</td><td>${escaparHtml(organizador.email)}</td><td><span class="status-dot ${organizador.ativo ? "active" : "inactive"}"></span> ${organizador.ativo ? "Ativo" : "Inativo"}</td></tr>`).join("");
}

async function salvarOrganizador(evento) {
    evento.preventDefault();
    const corpo = {
        nome: elementos.organizadorNome.value,
        email: elementos.organizadorEmail.value,
        senha: elementos.organizadorSenha.value
    };
    await salvar(`${apiBase}/organizadores`, "POST", corpo, "Organizador cadastrado com sucesso.", async () => {
        elementos.organizadorForm.reset();
        await carregarOrganizadores();
    }, elementos.organizadorForm);
}

async function carregarParticipantes() {
    const resultado = await requisicao(`${apiBase}/participantes`);
    participantes = resultado.dados;
    renderizarTabelaParticipantes();
    preencherSelectParticipantesInscricao();
}

function participanteTemInscricaoNoEvento(participanteId, eventoId) {
    return inscricoes.some(inscricao => {
        if (inscricao.participanteId !== participanteId) return false;
        const workshop = workshops.find(item => item.id === inscricao.workshopId);
        return String(workshop?.eventoId) === String(eventoId);
    });
}

function filtrarParticipantes() {
    const busca = normalizarTexto(elementos.buscaParticipante.value);
    const eventoId = elementos.filtroEvento.value;
    const status = elementos.filtroStatus.value;
    return participantes.filter(participante => {
        const texto = normalizarTexto(`${participante.nome} ${participante.email} ${participante.codigoInscricao}`);
        const okBusca = !busca || texto.includes(busca);
        const okEvento = !eventoId || participanteTemInscricaoNoEvento(participante.id, eventoId);
        const okStatus = !status || (status === "ativo" ? participante.ativo : !participante.ativo);
        return okBusca && okEvento && okStatus;
    });
}

function renderizarTabelaParticipantes() {
    const dados = filtrarParticipantes();
    if (!dados.length) {
        elementos.tabela.innerHTML = `<tr><td colspan="4" class="text-center py-4">Nenhum participante encontrado.</td></tr>`;
        return;
    }
    elementos.tabela.innerHTML = dados.map(participante => `<tr><td>${escaparHtml(participante.codigoInscricao)}</td><td>${escaparHtml(participante.nome)}<div class="small text-muted">${escaparHtml(participante.email)}</div></td><td><span class="status-dot ${participante.ativo ? "active" : "inactive"}"></span> ${participante.ativo ? "Ativo" : "Inativo"}</td><td class="text-end"><button class="btn btn-outline-secondary btn-sm" onclick="editarParticipante(${participante.id})">Editar</button> <button class="btn btn-outline-danger btn-sm" onclick="abrirExclusaoParticipante(${participante.id})">Excluir</button></td></tr>`).join("");
}

function editarParticipante(id) {
    const participante = participantes.find(item => item.id === id);
    if (!participante) return;
    elementos.participanteId.value = participante.id;
    elementos.nome.value = participante.nome;
    elementos.email.value = participante.email;
    elementos.inscricao.value = participante.codigoInscricao;
    elementos.inscricao.disabled = true;
    elementos.dataNascimento.value = participante.dataNascimento;
    elementos.ativo.checked = participante.ativo;
}

function limparFormulario() {
    elementos.form.reset();
    elementos.participanteId.value = "";
    elementos.inscricao.disabled = false;
    elementos.ativo.checked = true;
}

async function salvarParticipante(evento) {
    evento.preventDefault();
    const id = elementos.participanteId.value;
    const corpo = {
        nome: elementos.nome.value,
        email: elementos.email.value,
        dataNascimento: elementos.dataNascimento.value,
        ativo: elementos.ativo.checked
    };
    if (!id) corpo.codigoInscricao = elementos.inscricao.value;
    await salvar(`${apiBase}/participantes${id ? `/${id}` : ""}`, id ? "PUT" : "POST", corpo, id ? "Participante atualizado com sucesso." : "Participante cadastrado com sucesso.", async () => {
        limparFormulario();
        await carregarParticipantes();
    }, elementos.form);
}

async function carregarWorkshops() {
    const resultado = await requisicao(`${apiBase}/workshops`);
    workshops = resultado.dados;
    renderizarTabelaWorkshops();
    preencherSelectWorkshops();
}

function renderizarTabelaWorkshops() {
    if (!workshops.length) {
        elementos.workshopsTabela.innerHTML = `<tr><td colspan="5" class="text-center py-4">Nenhum workshop cadastrado.</td></tr>`;
        return;
    }
    elementos.workshopsTabela.innerHTML = workshops.map(workshop => `<tr><td>${escaparHtml(workshop.codigo)}</td><td>${escaparHtml(workshop.nome)}</td><td>${escaparHtml(workshop.eventoNome)}</td><td>${workshop.cargaHoraria}h</td><td class="text-end">${perfilAtual === "Administrador" ? `<button class="btn btn-outline-secondary btn-sm" onclick="editarWorkshop(${workshop.id})">Editar</button> <button class="btn btn-outline-danger btn-sm" onclick="abrirExclusaoWorkshop(${workshop.id})">Excluir</button>` : `<span class="text-muted">Somente leitura</span>`}</td></tr>`).join("");
}

function editarWorkshop(id) {
    const workshop = workshops.find(item => item.id === id);
    if (!workshop) return;
    elementos.workshopId.value = workshop.id;
    elementos.workshopNome.value = workshop.nome;
    elementos.workshopCodigo.value = workshop.codigo;
    elementos.workshopCargaHoraria.value = workshop.cargaHoraria;
    elementos.workshopEventoId.value = workshop.eventoId;
}

function limparFormularioWorkshop() {
    elementos.workshopForm.reset();
    elementos.workshopId.value = "";
}

async function salvarWorkshop(evento) {
    evento.preventDefault();
    const id = elementos.workshopId.value;
    const corpo = {
        nome: elementos.workshopNome.value,
        codigo: elementos.workshopCodigo.value,
        cargaHoraria: Number(elementos.workshopCargaHoraria.value),
        eventoId: Number(elementos.workshopEventoId.value)
    };
    await salvar(`${apiBase}/workshops${id ? `/${id}` : ""}`, id ? "PUT" : "POST", corpo, id ? "Workshop atualizado com sucesso." : "Workshop cadastrado com sucesso.", async () => {
        limparFormularioWorkshop();
        await carregarWorkshops();
    }, elementos.workshopForm);
}

function preencherSelectParticipantesInscricao() {
    const ativos = participantes.filter(participante => participante.ativo);
    elementos.inscricaoParticipanteId.innerHTML = `<option value="" disabled selected>Selecione um participante</option>${ativos.map(participante => `<option value="${participante.id}">${escaparHtml(participante.nome)} - ${escaparHtml(participante.codigoInscricao)}</option>`).join("")}`;
}

function preencherSelectWorkshops() {
    elementos.inscricaoWorkshopId.innerHTML = `<option value="" disabled selected>Selecione um workshop</option>${workshops.map(workshop => `<option value="${workshop.id}">${escaparHtml(workshop.eventoNome)} | ${escaparHtml(workshop.codigo)} - ${escaparHtml(workshop.nome)}</option>`).join("")}`;
}

async function carregarInscricoes() {
    const resultado = await requisicao(`${apiBase}/inscricoes`);
    inscricoes = resultado.dados;
    renderizarTabelaInscricoes();
}

async function carregarDadosParticipante() {
    await carregarEventosParticipante();
    await carregarWorkshopsParticipante();
    const resultado = await requisicao(`${apiBase}/inscricoes/minhas`);
    inscricoes = resultado.dados;
    renderizarAreaParticipante();
}

async function carregarEventosParticipante() {
    const resultado = await requisicao(`${apiBase}/eventos`);
    eventos = resultado.dados;
}

async function carregarWorkshopsParticipante() {
    const resultado = await requisicao(`${apiBase}/workshops`);
    workshops = resultado.dados;
}

function obterWorkshopDaInscricao(inscricao) {
    return workshops.find(workshop => workshop.id === inscricao.workshopId);
}

function obterEventoDaInscricao(inscricao) {
    const workshop = obterWorkshopDaInscricao(inscricao);
    return eventos.find(evento => evento.id === (workshop?.eventoId || inscricao.eventoId));
}

function statusParticipante(inscricao) {
    return inscricao.status === "Compareceu" ? "Confirmado" : inscricao.status;
}

function inscricaoHistoricoParticipante(inscricao) {
    const status = statusParticipante(inscricao);
    return status === "Confirmado" || status === "Concluido";
}

function cargaHorariaInscricao(inscricao) {
    return Number(obterWorkshopDaInscricao(inscricao)?.cargaHoraria || 0);
}

function filtrarInscricoesParticipante() {
    const busca = normalizarTexto(elementos.participanteBusca.value);
    const tipo = elementos.participanteFiltroTipo.value;
    const status = elementos.participanteFiltroStatus.value;

    return inscricoes.filter(inscricao => {
        const workshop = obterWorkshopDaInscricao(inscricao);
        const evento = obterEventoDaInscricao(inscricao);
        const texto = normalizarTexto(`${inscricao.eventoNome} ${inscricao.workshopNome} ${evento?.local || ""} ${workshop?.codigo || ""}`);
        const okBusca = !busca || texto.includes(busca);
        const okStatus = !status || statusParticipante(inscricao) === status;
        const okTipo = !tipo || (tipo === "eventos" ? Boolean(evento) : Boolean(workshop));
        return okBusca && okStatus && okTipo;
    });
}

function renderizarAreaParticipante() {
    const nome = participanteAtual?.nome || "participante";

    elementos.participanteHeaderNome.textContent = nome;
    elementos.participanteTitulo.textContent = `Olá, ${nome}`;

    const proximosEventos = new Set();
    inscricoes.forEach(inscricao => {
        const evento = obterEventoDaInscricao(inscricao);
        if (evento && new Date(`${evento.dataFim}T23:59:59`) >= new Date()) proximosEventos.add(evento.id);
    });

    const horasHistorico = inscricoes
        .filter(inscricaoHistoricoParticipante)
        .reduce((total, inscricao) => total + cargaHorariaInscricao(inscricao), 0);

    elementos.participanteMetricEventos.textContent = proximosEventos.size;
    elementos.participanteMetricWorkshops.textContent = inscricoes.length;
    elementos.participanteMetricHistorico.textContent = `${horasHistorico}h`;
    elementos.participanteHistoricoHoras.textContent = `${horasHistorico}h acumuladas`;

    renderizarCardsParticipante();
    renderizarHistoricoParticipante();
}

function renderizarCardsParticipante() {
    const dados = filtrarInscricoesParticipante();
    if (!dados.length) {
        elementos.participanteCards.innerHTML = `<div class="col-12"><div class="card"><div class="card-body text-center text-muted py-4">Nenhum evento ou workshop encontrado.</div></div></div>`;
        return;
    }

    elementos.participanteCards.innerHTML = dados.map(inscricao => {
        const evento = obterEventoDaInscricao(inscricao);
        const workshop = obterWorkshopDaInscricao(inscricao);
        const status = statusParticipante(inscricao);
        const confirmado = status === "Confirmado";
        return `<div class="col-12 col-lg-6"><article class="participant-event-card ${confirmado ? "is-confirmed" : ""}"><div class="d-flex justify-content-between gap-3 mb-2"><span class="badge ${confirmado ? "text-bg-primary" : "text-bg-secondary"}">${escaparHtml(status)}</span><small>${cargaHorariaInscricao(inscricao)}h</small></div><h2 class="h5 mb-1">${escaparHtml(inscricao.eventoNome)}</h2><p class="mb-2">${escaparHtml(inscricao.workshopNome)}</p><div class="participant-card-meta"><span>${escaparHtml(evento?.local || "Local a confirmar")}</span><span>${evento ? `${formatarDataSimples(evento.dataInicio)} a ${formatarDataSimples(evento.dataFim)}` : formatarData(inscricao.dataInscricao)}</span><span>${escaparHtml(workshop?.codigo || "Workshop")}</span></div>${confirmado ? `<div class="d-flex flex-wrap gap-2 mt-3"><button class="btn btn-primary btn-sm" type="button" onclick="verIngressoParticipante(${inscricao.id})">Ver ingresso</button><button class="btn btn-outline-primary btn-sm" type="button" onclick="adicionarAgendaParticipante(${inscricao.id})">Adicionar à agenda</button></div>` : ""}</article></div>`;
    }).join("");
}

function renderizarHistoricoParticipante() {
    const historico = inscricoes.filter(inscricaoHistoricoParticipante);
    elementos.participanteHistoricoTabela.innerHTML = historico.length
        ? historico.map(inscricao => `<tr><td>${escaparHtml(inscricao.eventoNome)}</td><td>${escaparHtml(inscricao.workshopNome)}</td><td>${renderizarStatusInscricao(statusParticipante(inscricao))}</td><td>${cargaHorariaInscricao(inscricao)}h</td></tr>`).join("")
        : `<tr><td colspan="4" class="text-center py-4">Nenhuma hora de capacitação acumulada ainda.</td></tr>`;
}

function verIngressoParticipante(id) {
    const inscricao = inscricoes.find(item => item.id === id);
    if (!inscricao) return;
    const evento = obterEventoDaInscricao(inscricao);
    mostrarMensagem(`Ingresso confirmado: ${inscricao.eventoNome} - ${inscricao.workshopNome}${evento ? ` em ${formatarDataSimples(evento.dataInicio)}` : ""}.`, "info");
}

function adicionarAgendaParticipante(id) {
    const inscricao = inscricoes.find(item => item.id === id);
    const evento = inscricao ? obterEventoDaInscricao(inscricao) : null;
    if (!inscricao || !evento) return;

    const inicio = evento.dataInicio.replaceAll("-", "");
    const fim = evento.dataFim.replaceAll("-", "");
    const conteudo = [
        "BEGIN:VCALENDAR",
        "VERSION:2.0",
        "BEGIN:VEVENT",
        `SUMMARY:${inscricao.eventoNome} - ${inscricao.workshopNome}`,
        `DTSTART;VALUE=DATE:${inicio}`,
        `DTEND;VALUE=DATE:${fim}`,
        `LOCATION:${evento.local || ""}`,
        "END:VEVENT",
        "END:VCALENDAR"
    ].join("\n");
    const link = document.createElement("a");
    link.href = `data:text/calendar;charset=utf-8,${encodeURIComponent(conteudo)}`;
    link.download = `agenda-${inscricao.id}.ics`;
    link.click();
}

function renderizarTabelaInscricoes() {
    if (!inscricoes.length) {
        elementos.inscricoesTabela.innerHTML = `<tr><td colspan="6" class="text-center py-4">Nenhuma inscricao realizada.</td></tr>`;
        return;
    }
    elementos.inscricoesTabela.innerHTML = inscricoes.map(inscricao => `<tr><td>${escaparHtml(inscricao.participanteNome)}</td><td>${escaparHtml(inscricao.eventoNome)}</td><td>${escaparHtml(inscricao.workshopNome)}</td><td>${formatarData(inscricao.dataInscricao)}</td><td>${renderizarStatusInscricao(inscricao.status)}</td><td class="text-end">${perfilAtual === "Administrador" || perfilAtual === "Organizador" ? renderizarAcoesInscricao(inscricao) : `<span class="text-muted">Somente leitura</span>`}</td></tr>`).join("");
}

async function salvarInscricao(evento) {
    evento.preventDefault();
    const corpo = {
        participanteId: Number(elementos.inscricaoParticipanteId.value),
        workshopId: Number(elementos.inscricaoWorkshopId.value)
    };
    await salvar(`${apiBase}/inscricoes`, "POST", corpo, "Inscrição realizada com sucesso.", async () => {
        elementos.inscricaoForm.reset();
        await carregarInscricoes();
    }, elementos.inscricaoForm);
}

function renderizarStatusInscricao(status) {
    const classes = {
        Inscrito: "text-bg-secondary",
        Compareceu: "text-bg-primary",
        Confirmado: "text-bg-primary",
        Concluido: "text-bg-success"
    };
    const classe = classes[status] || "text-bg-secondary";
    return `<span class="badge ${classe}">${escaparHtml(status)}</span>`;
}

function renderizarAcoesInscricao(inscricao) {
    const acoes = [];
    if (inscricao.status !== "Compareceu" && inscricao.status !== "Concluido") {
        acoes.push(`<button class="btn btn-outline-primary btn-sm" onclick="atualizarStatusInscricao(${inscricao.id}, 'Compareceu')">Compareceu</button>`);
    }
    if (inscricao.status !== "Concluido") {
        acoes.push(`<button class="btn btn-outline-success btn-sm" onclick="atualizarStatusInscricao(${inscricao.id}, 'Concluido')">Concluiu</button>`);
    }
    if (inscricao.status !== "Inscrito") {
        acoes.push(`<button class="btn btn-outline-secondary btn-sm" onclick="atualizarStatusInscricao(${inscricao.id}, 'Inscrito')">Reabrir</button>`);
    }
    if (perfilAtual === "Administrador") {
        acoes.push(`<button class="btn btn-outline-danger btn-sm" onclick="abrirExclusaoInscricao(${inscricao.id})">Excluir</button>`);
    }
    return acoes.join(" ");
}

async function atualizarStatusInscricao(id, status) {
    try {
        await requisicao(`${apiBase}/inscricoes/${id}/status`, {
            method: "PATCH",
            body: JSON.stringify({ status })
        });
        mostrarMensagem("Status da inscricao atualizado com sucesso.");
        await carregarInscricoes();
        renderizarTabelaParticipantes();
        renderizarDashboard();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    }
}

function inscricaoContaComoComparecimento(inscricao) {
    return inscricao.status === "Compareceu" || inscricao.status === "Confirmado" || inscricao.status === "Concluido";
}

async function salvar(url, metodo, corpo, mensagem, aoConcluir, form) {
    const botao = form.querySelector("button[type='submit']");
    definirCarregando(botao, true, "Salvando...");
    try {
        await requisicao(url, { method: metodo, body: JSON.stringify(corpo) });
        mostrarMensagem(mensagem);
        await aoConcluir();
        renderizarDashboard();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(botao, false);
    }
}

function contarInscricoesPorEvento(eventoId) {
    const participantesUnicos = new Set();
    inscricoes.forEach(inscricao => {
        const workshop = workshops.find(item => item.id === inscricao.workshopId);
        if (workshop?.eventoId === eventoId) participantesUnicos.add(inscricao.participanteId);
    });
    return participantesUnicos.size;
}

function renderizarDashboard() {
    const participantesAtivos = participantes.filter(participante => participante.ativo).length;
    const cargaHorariaTotal = workshops.reduce((total, workshop) => total + Number(workshop.cargaHoraria || 0), 0);
    const totalCompareceram = inscricoes.filter(inscricaoContaComoComparecimento).length;
    const totalConcluiram = inscricoes.filter(inscricao => inscricao.status === "Concluido").length;
    elementos.dashTotalParticipantes.textContent = participantes.length;
    elementos.dashParticipantesAtivos.textContent = `${participantesAtivos} ativos e ${participantes.length - participantesAtivos} inativos`;
    elementos.dashTotalEventos.textContent = eventos.length;
    elementos.dashMediaParticipantesEvento.textContent = `${inscricoes.length} inscrições em eventos`;
    elementos.dashTotalWorkshops.textContent = workshops.length;
    elementos.dashCargaHoraria.textContent = `${cargaHorariaTotal}h cadastradas`;
    elementos.dashTotalInscricoes.textContent = inscricoes.length;
    elementos.dashUltimaInscricao.textContent = `${totalCompareceram} compareceram, ${totalConcluiram} concluiram`;
    renderizarParticipantesPorEvento();
    renderizarResumoRelatorios(participantesAtivos, cargaHorariaTotal);
    renderizarUltimasInscricoes();
}

function renderizarParticipantesPorEvento() {
    if (!eventos.length) {
        elementos.dashParticipantesPorEvento.innerHTML = `<p class="text-muted mb-0">Cadastre eventos para visualizar a distribuição.</p>`;
        return;
    }
    const maior = Math.max(...eventos.map(evento => contarInscricoesPorEvento(evento.id)), 1);
    elementos.dashParticipantesPorEvento.innerHTML = eventos.map(evento => {
        const total = contarInscricoesPorEvento(evento.id);
        const largura = Math.round((total / maior) * 100);
        return `<div class="course-progress"><div class="d-flex justify-content-between gap-3"><span>${escaparHtml(evento.nome)}</span><strong>${total} participante${total === 1 ? "" : "s"}</strong></div><div class="progress"><div class="progress-bar" style="width: ${largura}%"></div></div></div>`;
    }).join("");
}

function renderizarResumoRelatorios(participantesAtivos, cargaHorariaTotal) {
    const eventoMaisParticipantes = eventos.map(evento => ({ nome: evento.nome, total: contarInscricoesPorEvento(evento.id) })).sort((a, b) => b.total - a.total)[0];
    const workshopMaiorCarga = workshops.slice().sort((a, b) => Number(b.cargaHoraria || 0) - Number(a.cargaHoraria || 0))[0];
    const totalCompareceram = inscricoes.filter(inscricaoContaComoComparecimento).length;
    const totalConcluiram = inscricoes.filter(inscricao => inscricao.status === "Concluido").length;
    const itens = [
        ["Participantes ativos", `${participantesAtivos} de ${participantes.length}`],
        ["Evento com mais participantes", eventoMaisParticipantes?.total ? `${eventoMaisParticipantes.nome} (${eventoMaisParticipantes.total})` : "Sem inscrições"],
        ["Comparecimento", `${totalCompareceram} de ${inscricoes.length}`],
        ["Conclusoes", `${totalConcluiram} de ${inscricoes.length}`],
        ["Maior carga horária", workshopMaiorCarga ? `${workshopMaiorCarga.nome} (${workshopMaiorCarga.cargaHoraria}h)` : "Sem workshops"],
        ["Carga horária total", `${cargaHorariaTotal}h`]
    ];
    elementos.dashResumoRelatorios.innerHTML = itens.map(([rotulo, valor]) => `<div class="list-group-item report-item"><span>${escaparHtml(rotulo)}</span><strong>${escaparHtml(valor)}</strong></div>`).join("");
}

function renderizarUltimasInscricoes() {
    const ultimas = inscricoes.slice().sort((a, b) => new Date(b.dataInscricao) - new Date(a.dataInscricao)).slice(0, 5);
    elementos.dashUltimasInscricoes.innerHTML = ultimas.length
        ? ultimas.map(inscricao => `<tr><td>${escaparHtml(inscricao.participanteNome)}</td><td>${escaparHtml(inscricao.eventoNome)}</td><td>${escaparHtml(inscricao.workshopNome)}</td><td>${formatarData(inscricao.dataInscricao)}</td><td>${renderizarStatusInscricao(inscricao.status)}</td></tr>`).join("")
        : `<tr><td colspan="5" class="text-center py-4">Nenhuma inscricao realizada.</td></tr>`;
}

function abrirExclusaoParticipante(id) {
    const participante = participantes.find(item => item.id === id);
    exclusaoAtual = { tipo: "participante", id };
    elementos.exclusaoTexto.textContent = "Deseja excluir este participante?";
    elementos.registroExclusaoNome.textContent = `${participante.nome} - inscrição ${participante.codigoInscricao}`;
    modalExclusao.show();
}

function abrirExclusaoEvento(id) {
    const evento = eventos.find(item => item.id === id);
    exclusaoAtual = { tipo: "evento", id };
    elementos.exclusaoTexto.textContent = "Deseja excluir este evento?";
    elementos.registroExclusaoNome.textContent = `${evento.codigo} - ${evento.nome}`;
    modalExclusao.show();
}

function abrirExclusaoWorkshop(id) {
    const workshop = workshops.find(item => item.id === id);
    exclusaoAtual = { tipo: "workshop", id };
    elementos.exclusaoTexto.textContent = "Deseja excluir este workshop?";
    elementos.registroExclusaoNome.textContent = `${workshop.codigo} - ${workshop.nome}`;
    modalExclusao.show();
}

function abrirExclusaoInscricao(id) {
    const inscricao = inscricoes.find(item => item.id === id);
    exclusaoAtual = { tipo: "inscricao", id };
    elementos.exclusaoTexto.textContent = "Deseja excluir esta inscrição?";
    elementos.registroExclusaoNome.textContent = `${inscricao.participanteNome} - ${inscricao.workshopNome}`;
    modalExclusao.show();
}

async function confirmarExclusao() {
    if (!exclusaoAtual) return;
    const rotas = { participante: "participantes", evento: "eventos", workshop: "workshops", inscricao: "inscricoes" };
    try {
        await requisicao(`${apiBase}/${rotas[exclusaoAtual.tipo]}/${exclusaoAtual.id}`, { method: "DELETE" });
        mostrarMensagem("Registro excluído com sucesso.");
        modalExclusao.hide();
        exclusaoAtual = null;
        await carregarDadosHome();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    }
}

async function atualizarDados() {
    definirCarregando(elementos.btnAtualizar, true, "Atualizando...");
    try {
        await carregarDadosHome();
        mostrarMensagem("Dados atualizados.");
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(elementos.btnAtualizar, false);
    }
}

function abrirAbaPorSeletor(seletor) {
    const botao = document.querySelector(`[data-bs-target="${seletor}"]`);
    if (botao) bootstrap.Tab.getOrCreateInstance(botao).show();
}

elementos.loginForm.addEventListener("submit", login);
elementos.olhoSenha.addEventListener('mousedown', function () { elementos.senha.type = 'text'; });
elementos.olhoSenha.addEventListener('mouseup', function () { elementos.senha.type = 'password'; });
elementos.olhoSenha.addEventListener('mousemove', function () { elementos.senha.type = 'password'; });
elementos.cadastroContaForm.addEventListener("submit", cadastrarContaParticipante);
elementos.btnLogout.addEventListener("click", logout);
elementos.btnLogoutParticipante.addEventListener("click", logout);
elementos.btnAtualizar.addEventListener("click", atualizarDados);
elementos.btnCancelar.addEventListener("click", limparFormulario);
elementos.form.addEventListener("submit", salvarParticipante);
elementos.eventoForm.addEventListener("submit", salvarEvento);
elementos.btnCancelarEvento.addEventListener("click", limparFormularioEvento);
elementos.organizadorForm.addEventListener("submit", salvarOrganizador);
elementos.workshopForm.addEventListener("submit", salvarWorkshop);
elementos.btnCancelarWorkshop.addEventListener("click", limparFormularioWorkshop);
elementos.inscricaoForm.addEventListener("submit", salvarInscricao);
elementos.buscaParticipante.addEventListener("input", renderizarTabelaParticipantes);
elementos.filtroEvento.addEventListener("change", renderizarTabelaParticipantes);
elementos.filtroStatus.addEventListener("change", renderizarTabelaParticipantes);
elementos.participanteBusca.addEventListener("input", renderizarCardsParticipante);
elementos.participanteFiltroTipo.addEventListener("change", renderizarCardsParticipante);
elementos.participanteFiltroStatus.addEventListener("change", renderizarCardsParticipante);
elementos.btnConfirmarExclusao.addEventListener("click", confirmarExclusao);
document.querySelectorAll("[data-ir-aba]").forEach(botao => botao.addEventListener("click", () => abrirAbaPorSeletor(botao.dataset.irAba)));

if (token) {
    const inicio = perfilAtual === "Participante" ? iniciarAreaParticipante : iniciarHome;
    inicio().catch(erro => mostrarMensagem(erro.message, "danger"));
} else {
    mostrarTelaLogin();
}