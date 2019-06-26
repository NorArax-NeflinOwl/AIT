package structures;

public class AitClientData {
    private Integer id;
    private String login;
    private String password;

    private boolean isConnected;
    private boolean isLoggedIn;
    private boolean isWaiting;

    public  AitClientData(Integer id) {
        this.id = id;
        isConnected = true;
        isLoggedIn = false;
        isWaiting = false;
    }

    public Integer getId() { return this.id; }

    public String getLogin() { return this.login; }
    public void setLogin(String login) { this.login = login; }

    public String getPassword() { return this.password; }
    public void setPassword(String pass) { this.password = pass; }

    public boolean getIsConnected() { return this.isConnected; }
    public void setIsConnected() { this.isConnected = !this.isConnected; }

    public boolean getIsLoggedIn() { return this.isLoggedIn; }
    public void setIsLoggedIn() { this.isLoggedIn = !this.isLoggedIn; }

    public boolean getIsWaiting() { return this.isWaiting; }
    public void setIsWaiting() { this.isWaiting = !this.isWaiting; }

    public boolean canProccessing() { return this.isConnected && this.isLoggedIn; }
}
