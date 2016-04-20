package spacetraffic.kiv.zcu.cz.logoquiz;

import android.app.ProgressDialog;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.ActivityInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import org.ksoap2.serialization.SoapObject;

import java.util.Iterator;
import java.util.List;
import java.util.Random;
import java.util.Timer;
import java.util.TimerTask;

import spacetraffic.kiv.zcu.cz.gameelement.Answer;
import spacetraffic.kiv.zcu.cz.gameelement.Answers;
import spacetraffic.kiv.zcu.cz.gameelement.Question;
import spacetraffic.kiv.zcu.cz.gameelement.Result;
import spacetraffic.kiv.zcu.cz.gameelement.ResultState;

/**
 * Game activity class. The main activity of game.
 */
public class GameActivity extends AppCompatActivity {

    /**
     * Action for broadcast intent.
     */
    private static final String ACTION = "android.net.conn.CONNECTIVITY_CHANGE";

    /**
     * Interval for timer.
     */
    private static final long INTERVAL = 500;

    /**
     * Start time for timer.
     */
    private static final long START_TIME = 40000;

    /**
     * Interval for request timer.
     */
    private static final long REQUEST_INTERVAL = 25000;

    /**
     * Delay for request timer.
     */
    private static final long REQUEST_DELAY = 0;

    /**
     * Progress dialog.
     */
    private ProgressDialog progressDialog;

    /**
     * Alert dialog factory reference.
     */
    private AlertDialogFactory alertDialogFactory;

    /**
     * Player id.
     */
    private int playerId;

    /**
     * Game id.
     */
    private int gameId;

    /**
     * Indication if player wins.
     */
    private boolean win = false;

    /**
     * First answer button.
     */
    private Button answer1Button;

    /**
     * Second answer button.
     */
    private Button answer2Button;

    /**
     * Third answer button.
     */
    private Button answer3Button;

    /**
     * Logo image view.
     */
    private ImageView logoImageView;

    /**
     * Question text view (It contains actual number of questin from all questions).
     */
    private TextView questionTextView;

    /**
     * Countdown timer text view.
     */
    private TextView timerTextView;

    /**
     * List of questions.
     */
    private List<Question> questions;

    /**
     * Answers.
     */
    private Answers answers = new Answers();

    /**
     * Current question.
     */
    private Question currentQuestion;

    /**
     * Iterator of question collections
     */
    private Iterator<Question> questionIterator;

    /**
     * Random.
     */
    private Random random = new Random();

    /**
     * Countdown timer.
     */
    private MyCountDownTimer timer;

    /**
     * Request timer.
     */
    private Timer requestTimer;

    /**
     * Asynchronous request task.
     */
    private MyAsyncTask requesterTask;

    /**
     * Internet broadcast receiver.
     */
    private InternetBroadcastReceiver internetBroadcastReceiver = new InternetBroadcastReceiver(this);

    /**
     * Indication if IBR is registered or not.
     */
    private boolean internetBroadcastReceiverRegistered = false;

    /**
     * Overriden onCreate method.
     * @param savedInstanceState
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        requestWindowFeature(Window.FEATURE_NO_TITLE);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setRequestedOrientation (ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);

        setContentView(R.layout.activity_game);

        registerReceiver();
        this.alertDialogFactory = new AlertDialogFactory(this);

        getBundle();
        getFields();
        prepareGetQuestion();
    }

    /**
     * Method for registering receiver.
     */
    private void registerReceiver(){
        if(!this.internetBroadcastReceiverRegistered){
            IntentFilter filter = new IntentFilter(ACTION);
            this.registerReceiver(internetBroadcastReceiver, filter);

            this.internetBroadcastReceiver.prepareDialog();
            this.internetBroadcastReceiverRegistered = true;
        }
    }

    /**
     * Method for unregistering receiver.
     */
    public void unregisterReceiver(){
        if(this.internetBroadcastReceiverRegistered){
            this.unregisterReceiver(internetBroadcastReceiver);
            this.internetBroadcastReceiverRegistered = false;
        }
    }

    /**
     * Method for get bundle.
     */
    private void getBundle(){
        Intent thisIntent = getIntent();
        this.gameId = thisIntent.getIntExtra("gameId", 0);
        this.playerId = thisIntent.getIntExtra("playerId", 0);
    }

    /**
     * Method for getting text views and image views.
     */
    private void getFields(){
        this.answer1Button = (Button) findViewById(R.id.answer1Button);
        this.answer2Button = (Button) findViewById(R.id.answer2Button);
        this.answer3Button = (Button) findViewById(R.id.answer3Button);
        this.logoImageView = (ImageView) findViewById(R.id.logoImageView);
        this.questionTextView = (TextView) findViewById(R.id.questionTextView);
        this.timerTextView = (TextView) findViewById(R.id.timerTextView);
    }

    /**
     * Method for hide progress dialog.
     */
    private void hideProgressDialog(){
        if(this.progressDialog != null && this.progressDialog.isShowing())
            progressDialog.dismiss();
    }

    /**
     * Method for dissmiss all timers and unregistered recevier.
     */
    public void dismissAll(){
        hideProgressDialog();
        cancelRequestTimer();

        unregisterReceiver();
    }

    /**
     * Method for preparing message for get questions.
     */
    private void prepareGetQuestion(){
        this.progressDialog = ProgressDialog.show(this, getResources().getString(R.string.progress_logo_generate_title),
                getResources().getString(R.string.progress_logo_generate_text), true);

        Sender sender = new Sender("performAction");
        sender.addProperty("minigameId", this.gameId);
        sender.addProperty("actionName", "getQuestions");

        MyAsyncTask lat = new MyAsyncTask(new AfterAction("getQuestion"));
        lat.execute(sender);
    }

    /**
     * Callback method for get question.
     * @param result result
     */
    private void getQuestion(Object result){
        Object retVal = checkResult(result);

        if(result != null){
            this.questions = Question.getListFromSoapObject((SoapObject) retVal);
            if(this.questions == null || this.questions.size() == 0){
                dismissAll();
                alertDialogFactory.showUnexpectedErrorDialog();
                return;
            }

            this.questionIterator = this.questions.iterator();

            setQuestion();
            hideProgressDialog();

            startRequestTimer();
            alertDialogFactory.showInfoDialog();
        }
    }

    /**
     * Method for check result.
     * @param result result
     * @return return return value or null
     */
    private Object checkResult(Object result) {
        if (result == null || !(result instanceof SoapObject)) {
            dismissAll();
            alertDialogFactory.showUnexpectedErrorDialog();
            return null;
        }

        Result res = new Result((SoapObject) result);

        if (res.getState() == ResultState.FAILURE || res.getReturnValue() == null){
            String title = getResources().getString(R.string.unexpected_error_title);
            dismissAll();
            alertDialogFactory.showFinishDialog(title, res.getMessage());
            return null;
        }

        return res.getReturnValue();
    }

    /**
     * Method for setting question.
     */
    private void setQuestion(){
        this.currentQuestion = this.questionIterator.next();

        this.logoImageView.setImageResource(getResources().getIdentifier(
                this.currentQuestion.getRightChoice().getImageName().replace(".png",""), "drawable", getPackageName()));
        setAnswers();

        this.questionTextView.setText(String.format("%s %d/%d",getResources().getString(R.string.question),
                this.currentQuestion.getId() + 1, this.questions.size()));
    }

    /**
     * Method for set next question. It is onClick method for answers.
     * @param v view (button)
     */
    public void nextQuestion(View v){
        Button b = (Button)v;
        String chooseAnswer = b.getText().toString();

        this.answers.add(new Answer(this.currentQuestion.getId(), chooseAnswer));

        if(this.questionIterator.hasNext()) {
            setQuestion();
            startTimer();
        }
        else {
            this.timer.pause();
            prepareSendGameResult();
        }
    }

    /**
     * Method for set answers.
     */
    private void setAnswers(){
        int randomNum = this.random.nextInt(3);
        switch (randomNum){
            case 0:
                this.answer1Button.setText(this.currentQuestion.getRightChoice().getName());
                this.answer2Button.setText(this.currentQuestion.getFirstWrongChoice());
                this.answer3Button.setText(this.currentQuestion.getSecondWrongChoice());
                break;
            case 1:
                this.answer1Button.setText(this.currentQuestion.getFirstWrongChoice());
                this.answer2Button.setText(this.currentQuestion.getRightChoice().getName());
                this.answer3Button.setText(this.currentQuestion.getSecondWrongChoice());
                break;
            case 2:
                this.answer1Button.setText(this.currentQuestion.getFirstWrongChoice());
                this.answer2Button.setText(this.currentQuestion.getSecondWrongChoice());
                this.answer3Button.setText(this.currentQuestion.getRightChoice().getName());
                break;
        }
    }

    /**
     * Method for start timer.
     */
    public void startTimer(){
        pauseTimer();

        this.timer = new MyCountDownTimer(START_TIME, INTERVAL, this.timerTextView,
                getResources().getString(R.string.remain), new OverTimeLimit());
        resumeTimer();
    }

    /**
     * Method for pause timer.
     */
    private void pauseTimer(){
        if(this.timer != null)
            this.timer.pause();
    }

    /**
     * Method for resume timer.
     */
    public void resumeTimer(){
        if(this.timer != null)
            this.timer.resume();
    }

    /**
     * Overriden onPause method.
     */
    @Override
    public void onPause(){
        super.onPause();
        pauseTimer();
    }

    /**
     * Overriden onResume method.
     */
    @Override
    public void onResume(){
        super.onResume();
        resumeTimer();
    }

    /**
     * Overriden onDestroy method.
     */
    @Override
    public void onDestroy(){
        super.onDestroy();

        cancelRequestTimer();
        unregisterReceiver();
    }

    /**
     * Overriden onBackPressed method.
     */
    @Override
    public void onBackPressed() {
        pauseTimer();
        alertDialogFactory.showFinishConfirmDialog();
    }

    /**
     * Method for start request timer.
     */
    private void startRequestTimer(){
        this.requestTimer = new Timer();
        this.requestTimer.schedule(new TimerTask() {
            @Override
            public void run() {
                Sender sender = new Sender("checkMinigameLifeAndUpdateLastRequestTime");
                sender.addProperty("minigameId", gameId);

                GameActivity.this.requesterTask = new MyAsyncTask(new AfterAction("checkAliveGame"));
                GameActivity.this.requesterTask.execute(sender);
            }
        }, REQUEST_DELAY, REQUEST_INTERVAL);
    }

    /**
     * Method for pause request timer.
     */
    public void pauseWithRequester(){
        pauseTimer();
        cancelRequestTimer();
    }

    /**
     * Method for resume request timer.
     */
    public void resumeWithRequester(){
        resumeTimer();
        startRequestTimer();
    }

    /**
     * Method for cancel request timer.
     */
    private void cancelRequestTimer(){
        if(this.requestTimer != null) {
            this.requestTimer.cancel();
        }

        if(this.requesterTask != null && this.requesterTask.getStatus() == AsyncTask.Status.RUNNING)
            this.requesterTask.cancel(true);
    }

    /**
     * Callback method for check alive game.
     * @param result result.
     */
    private void checkAliveGame(Object result){
        this.requesterTask = null;

        Object retVal = checkResult(result);
        if(retVal != null && !((Boolean) retVal)){
            this.timer.pause();
            dismissAll();
            alertDialogFactory.showNotAliveDialog();
        }
    }

    /**
     * Method for prepare send game result message to server.
     */
    private void prepareSendGameResult(){
        this.progressDialog = ProgressDialog.show(this, getResources().getString(R.string.progress_evaluate_title),
                getResources().getString(R.string.progress_evaluate_text), true);

        Sender sender = new Sender("checkAnswersSupportMethod");
        sender.addProperty("minigameId", this.gameId);

        String answersXml = this.answers.getXml();
        sender.addProperty("answersXml", answersXml);

        MyAsyncTask mat = new MyAsyncTask(new AfterAction("checkWin"));
        mat.execute(sender);
    }

    /**
     * Method for check if player wins.
     * @param result result
     */
    private void checkWin(Object result){
        Object retVal = checkResult(result);
        if(retVal != null){
            this.win = (Boolean) retVal;
            prepareFinish();
        }
    }

    /**
     * Method for prepare reward message to server.
     */
    private void prepareReward(){
        Sender sender = new Sender("rewardPlayer");
        sender.addProperty("minigameId", this.gameId);
        sender.addProperty("playerId", this.playerId);

        MyAsyncTask mat = new MyAsyncTask(new AfterAction("removeGame"));
        mat.execute(sender);
    }

    public void prepareFinish(){
        Sender sender = new Sender("endGame");
        sender.addProperty("minigameId", this.gameId);

        AfterAction afterAction = this.win ? new AfterAction("rewardPlayer") : new AfterAction("removeGame");

        MyAsyncTask mat = new MyAsyncTask(afterAction);
        mat.execute(sender);
    }

    /**
     * Method for prepare remove game message to server.
     */
    private void prepareRemoveGame(){
        Sender sender = new Sender("removeGame");
        sender.addProperty("minigameId", this.gameId);

        MyAsyncTask mat = new MyAsyncTask(new AfterAction("finishGame"));
        mat.execute(sender);
    }

    /**
     * Method for finishing game.
     */
    private void finishGame(){
        hideProgressDialog();
        pauseTimer();

        cancelRequestTimer();
        unregisterReceiver();

        if(win)
            alertDialogFactory.showWinDialog(getNumberOfCorrectAnswers(), this.questions.size());
        else
            alertDialogFactory.showLooseDialog(getNumberOfCorrectAnswers(), this.questions.size());
    }

    /**
     * Method for getting number of correct answers.
     * @return
     */
    private int getNumberOfCorrectAnswers(){
        Iterator<Answer> it = this.answers.iterator();

        int correctAnswers = 0;
        Answer answer;
        String rightChoice;

        while(it.hasNext()){
            answer = it.next();
            rightChoice = this.questions.get(answer.getId()).getRightChoice().getName();

            if(answer.getSelectedAnswer().compareTo(rightChoice) == 0){
                correctAnswers++;
            }
        }
        return correctAnswers;
    }

    /**
     * Class with calling callback methods.
     */
    private class AfterAction implements IPostAction {

        /**
         * Method name.
         */
        private String method;

        /**
         * AfterAction constructor.
         * @param method method name
         */
        public AfterAction(String method){
            this.method = method;
        }

        /**
         * Overriden doAction. It is calling callback methods by name.
         * @param result result
         */
        @Override
        public void doAction(Object result) {
            switch (method){
                case "getQuestion":
                    getQuestion(result);
                    break;
                case "checkWin":
                    checkWin(result);
                    break;
                case "rewardPlayer":
                    prepareReward();
                    break;
                case "removeGame":
                    prepareRemoveGame();
                    break;
                case "finishGame":
                    finishGame();
                    break;
                case "checkAliveGame":
                    checkAliveGame(result);
                    break;
            }
        }
    }

    /**
     * Class with callback for over time limit.
     */
    private class OverTimeLimit implements IPostAction{

        @Override
        public void doAction(Object result) {
            GameActivity.this.progressDialog = ProgressDialog.show(GameActivity.this,
                    getResources().getString(R.string.progress_timeup_title),
                    getResources().getString(R.string.progress_timeup_text), true);
            prepareFinish();
        }
    }
}
