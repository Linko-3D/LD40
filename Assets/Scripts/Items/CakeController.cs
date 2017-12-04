public class CakeController : ItemController, IController {
    
    public override void OnConsumedBy(PrincessCakeController controller) {
        base.OnConsumedBy(controller);

        controller.Model.EatCake();
    }

}
