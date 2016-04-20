package spacetraffic.kiv.zcu.cz.logoquiz;

import android.app.ProgressDialog;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.ActivityInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.TextView;
import android.widget.Toast;

import org.ksoap2.serialization.SoapObject;

import spacetraffic.kiv.zcu.cz.gameelement.MinigamePasswordHasher;
import spacetraffic.kiv.zcu.cz.gameelement.Result;
import spacetraffic.kiv.zcu.cz.gameelement.ResultState;

/**
 * LogInActivity class.
 */
public class LogInActivity extends AppCompatActivity {

    /**
     * Action for InternetBroadcastReceiver.
     */
    private static final String ACTION = "android.net.conn.CONNECTIVITY_CHANGE";

    /**
     * User text view.
     */
    private TextView userTv;

    /**
     * Password text view.
     */
    private TextView passwordTv;

    /**
     * Game id text view.
     */
    private TextView gameIdTv;

    /**
     * Username.
     */
    private String username = "";

    /**
     * Password.
     */
    private String password = "";

    /**
     * Minigame id.
     */
    private int gameId = -1;

    /**
     * Player id.
     */
    private int playerId = -1;

    /**
     * Password hasher.
     */
    private MinigamePasswordHasher hasher = new MinigamePasswordHasher();

    /**
     * Progress dialog.
     */
    private ProgressDialog progressDialog;

    /**
     * Internet broadcast receiver.
     */
    private InternetBroadcastReceiver internetBroadcastReceiver = new InternetBroadcastReceiver(this);

    /**
     * Indication if IBR is registered or not.
     */
    private boolean internetBroadcastReceiverRegistered = false;

    /**
     * On create method.
     * @param savedInstanceState
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        requestWindowFeature(Window.FEATURE_NO_TITLE);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
                WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setRequestedOrientation (ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);

        setContentView(R.layout.activity_log_in);

        getFields();
        registerReciever();
    }

    /**
     * Method for registering receiver.
     */
    private void registerReciever(){
        if(!this.internetBroadcastReceiverRegistered) {
            IntentFilter filter = new IntentFilter(ACTION);
            this.registerReceiver(internetBroadcastReceiver, filter);
            this.internetBroadcastReceiver.prepareDialog();
            this.internetBroadcastReceiverRegistered = true;
        }
    }

    /**
     * Method for unregistering receiver.
     */
    private void unregisterReceiver(){
        if(this.internetBroadcastReceiverRegistered){
            this.unregisterReceiver(this.internetBroadcastReceiver);
            this.internetBroadcastReceiverRegistered = false;
        }
    }

    /**
     * Method for getting all field by id.
     */
    private void getFields(){
        this.userTv = (TextView)findViewById(R.id.usernameTextView);
        this.passwordTv = (TextView)findViewById(R.id.passwordTextView);
        this.gameIdTv = (TextView)findViewById(R.id.gameIdTextView);
    }

    /**
     * Method for starting game.
     * @param v view
     */
    public void start(View v){
        this.progressDialog = ProgressDialog.show(this, getResources().getString(R.string.login_progress_title),
                getResources().getString(R.string.progress_message), true);

        if(!parseFields()){
            toastAndDismiss(R.string.empty_fields);
            return;
        }

        runGame();
    }

    /**
     * Method for parsin text from fields.
     * @return true if, parsing finished without complication.
     */
    private boolean parseFields(){
        this.username = this.userTv.getText().toString().trim();
        this.password = this.passwordTv.getText().toString();

        try {
            this.gameId = Integer.parseInt(this.gameIdTv.getText().toString());
        }
        catch (NumberFormatException e){
            Log.d(this.getClass().getSimpleName(), e.getMessage());
            return false;
        }

        return !(this.username.isEmpty() || this.password.isEmpty() || this.gameId == 0);
    }

    /**
     * Method for run game.
     */
    private void runGame(){
        //hash password
        new AsyncTask<Void, Void, String>(){

            @Override
            protected String doInBackground(Void... params) {
                try {
                    return hasher.getEncryptedPassword(password);
                } catch (Exception e) {
                    Log.e(this.getClass().getSimpleName(),e.getMessage());
                    return null;
                }
            }

            @Override
            protected void onPostExecute(String result){
                if(result == null)
                    toastAndDismiss(R.string.encryption_error);
                else
                    sendAuthentication(result);
            }
        }.execute();
    }

    /**
     * Method for sending authentication message.
     * @param encryptedPassword encrypted password
     */
    private void sendAuthentication(String encryptedPassword){
        Sender sender = new Sender("authenticatePlayerForMinigame");
        sender.addProperty("userName", this.username);
        sender.addProperty("passwd", encryptedPassword);

        MyAsyncTask mat = new MyAsyncTask(new AuthenticatePlayer());
        mat.execute(sender);
    }

    /**
     * Method for dismis progress dialog and show toast.
     * @param resourceString resource string id
     */
    private void toastAndDismiss(int resourceString){
        progressDialog.dismiss();
        Toast.makeText(this, resourceString, Toast.LENGTH_SHORT).show();
    }

    /**
     * Method for dismis progress dialog and show toast.
     * @param message message
     */
    private void toastAndDismiss(String message){
        progressDialog.dismiss();
        Toast.makeText(this, message, Toast.LENGTH_SHORT).show();
    }

    /**
     * Method for check result after authenticate player.
     * @param result result
     */
    private void authenticatePlayer(Object result){
        if(result == null) {
            toastAndDismiss(R.string.unexpected_error);
            return;
        }

        try {
            this.playerId = Integer.parseInt(result.toString());
        }
        catch (NumberFormatException e){
            Log.e(this.getClass().getSimpleName(), e.getMessage());
            toastAndDismiss(R.string.unexpected_error);
            return;
        }

        if(this.playerId == -1)
            toastAndDismiss(R.string.player_not_exsits);
        else
            prepareAddPlayer();
    }

    /**
     * Method for preparing send add player server.
     */
    private void prepareAddPlayer(){
        Sender sender = new Sender("addPlayer");
        sender.addProperty("minigameId", this.gameId);
        sender.addProperty("playerId", this.playerId);

        MyAsyncTask mat = new MyAsyncTask(new AddPlayer());
        mat.execute(sender);
    }

    /**
     * Method for check result message from server.
     * @param result result
     * @param startGame true if game should by started
     */
    private void checkResult(Object result, boolean startGame){
        if(result == null || !(result instanceof SoapObject)) {
            toastAndDismiss(R.string.unexpected_error);
            return;
        }

        Result res = new Result((SoapObject) result);

        if(res.getState() == ResultState.FAILURE)
            toastAndDismiss(res.getMessage());
        else{
            if(startGame)
                startGame();
            else
                prepareStartGame();
        }
    }

    /**
     * Method for preparing send start game to server.
     */
    private void prepareStartGame(){
        Sender sender = new Sender("startGame");
        sender.addProperty("minigameId", this.gameId);

        MyAsyncTask lat = new MyAsyncTask(new StartGame());
        lat.execute(sender);
    }

    /**
     * Method for start game.
     */
    private void startGame(){
        Intent callIntent = new Intent(this, GameActivity.class);

        Bundle bundle = new Bundle();

        bundle.putInt("playerId", this.playerId);
        bundle.putInt("gameId", this.gameId);

        callIntent.putExtras(bundle);
        startActivity(callIntent);

        unregisterReceiver();
        this.progressDialog.dismiss();
        this.finish();
    }

    /**
     * Overriden on destroy method.
     */
    @Override
    public void onDestroy(){
        super.onDestroy();
        unregisterReceiver();
    }

    /**
     * Inner class as callback on player authentication.
     */
    private class AuthenticatePlayer implements IPostAction {
        @Override
        public void doAction(Object result) {
            authenticatePlayer(result);
        }
    }

    /**
     * Inner class as callback on add player.
     */
    private class AddPlayer implements IPostAction {
        @Override
        public void doAction(Object result) {
            checkResult(result, false);
        }
    }

    /**
     * Inner class as callback on start game.
     */
    private class StartGame implements IPostAction {
        @Override
        public void doAction(Object result) {
            checkResult(result, true);
        }
    }

}
