Vagrant.configure("2") do |config|

  # --- Глобальні змінні ---
  baget_host_ip = "192.168.56.1"
  baget_port    = "5555"
  baget_source  = "http://#{baget_host_ip}:#{baget_port}/v3/index.json"
  
  tool_name     = "EduPlan.TaskCLI.App"
  tool_version  = "1.0.0"

  # --- Налаштування мережі Host-Only ---
  config.vm.network "private_network", ip: "192.168.56.10"

  # --- Спільна частина для обох VM ---
  provision_script = <<-SHELL
    # Додавання репозиторію Microsoft та встановлення .NET SDK
    wget --no-check-certificate https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    
    if [ ! -s packages-microsoft-prod.deb ]; then
      echo "Error: packages-microsoft-prod.deb is empty or zero size. Aborting installation."
      exit 1
    fi
    
    dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    
    apt update -y || true
    apt install -y dotnet-sdk-8.0
    
    # Видалення всіх існуючих джерел NuGet
    dotnet nuget disable source nuget.org
    dotnet nuget remove source nuget.org
    
    # 1. Додавання локального BaGet як джерела пакетів
    echo "Adding BaGet source: #{baget_source}"
    dotnet nuget add source "#{baget_source}" -n "BaGetLocal"
    
    # 2. Встановлення .NET Global Tool з локального джерела
    echo "Installing global tool: #{tool_name} v#{tool_version} from BaGetLocal"
    dotnet tool install --global #{tool_name} --version #{tool_version} --add-source "BaGetLocal"
    
    # 3. Перевірка та запуск інструмента
    echo "--- Running the installed tool (taskcli) ---"
    ~/.dotnet/tools/taskcli list
    
    echo "Provisioning complete on $(hostname)"
  SHELL

  # --- VM 1: Ubuntu ---
  config.vm.define "ubuntu-vm" do |ubuntu|
    ubuntu.vm.box = "generic/ubuntu2204"
    ubuntu.vm.hostname = "ubuntu-vm"
    ubuntu.vm.provision "shell", inline: provision_script
    ubuntu.vm.provider "virtualbox" do |vb|
      vb.memory = "2048"
    end
  end

  # --- VM 2: Debian ---
  config.vm.define "debian-vm" do |debian|
    debian.vm.box = "generic/debian12"
    debian.vm.hostname = "debian-vm"
    debian.vm.provision "shell", inline: provision_script
    debian.vm.provider "virtualbox" do |vb|
      vb.memory = "2048"
    end
  end
end